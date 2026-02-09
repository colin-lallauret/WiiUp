using UnityEngine;
using StarterAssets;

public class BoulderSpawner : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject cubePrefab;    
    public Transform[] spawnPoints; 
    public float spawnRate = 1.0f;  
    
    [Header("Limites")]
    public int maxCubesInScene = 20; 

    [Header("UI Feedback")]
    public GameObject blockOverlayImage;

    private float _nextSpawnTime;
    private bool _isPlayerInZone = false;

    // COMPTEUR PARTAGÉ (Static pour être vu par tous les scripts)
    public static int globalZoneCount = 0;

    private void OnEnable() { ThirdPersonController.OnPlayerRespawnEvent += ForceUnlockActions; }
    private void OnDisable() { ThirdPersonController.OnPlayerRespawnEvent -= ForceUnlockActions; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerInZone = true;
            globalZoneCount++; // On ajoute une zone
            UpdateGlobalAbilities(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerInZone = false;
            globalZoneCount--; // On retire une zone
            if (globalZoneCount < 0) globalZoneCount = 0;
            UpdateGlobalAbilities(other.gameObject);
        }
    }

    void Update()
    {
        if (_isPlayerInZone && Time.time >= _nextSpawnTime)
        {
            if (cubePrefab != null && spawnPoints.Length > 0)
            {
                if (GameObject.FindGameObjectsWithTag("Obstacle").Length < maxCubesInScene)
                {
                    SpawnCube();
                }
                _nextSpawnTime = Time.time + spawnRate;
            }
        }
    }

    void SpawnCube()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        GameObject newCube = Instantiate(cubePrefab, spawnPoints[randomIndex].position, Random.rotation);
        newCube.tag = "Obstacle"; 
        Rigidbody rb = newCube.GetComponent<Rigidbody>();
        if (rb != null) rb.WakeUp();
        Destroy(newCube, 10f);
    }

    private void UpdateGlobalAbilities(GameObject player)
    {
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            bool shouldBeEnabled = (globalZoneCount == 0);
            controller.canThrowGrenade = shouldBeEnabled;
            controller.canUseParachute = shouldBeEnabled;
            if (blockOverlayImage != null) blockOverlayImage.SetActive(!shouldBeEnabled);
        }
    }

    private void ForceUnlockActions()
    {
        globalZoneCount = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) UpdateGlobalAbilities(player);
        _isPlayerInZone = false;
    }
}