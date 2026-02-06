using UnityEngine;
using StarterAssets;
using UnityEngine.UI; // Nécessaire pour manipuler les images

public class BoulderSpawner : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject cubePrefab;    
    public Transform[] spawnPoints; 
    public float spawnRate = 1.0f;  
    
    [Header("Limites")]
    public int maxCubesInScene = 20; 

    [Header("UI Feedback")]
    public GameObject blockOverlayImage; // Glisse ton image "BlockOverlay" ici

    private float _nextSpawnTime;
    private bool _isPlayerInZone = false;

    private void OnEnable()
    {
        ThirdPersonController.OnPlayerRespawnEvent += ForceUnlockActions;
    }

    private void OnDisable()
    {
        ThirdPersonController.OnPlayerRespawnEvent -= ForceUnlockActions;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerInZone = true;
            SetPlayerAbilities(other.gameObject, false);
            
            // AFFICHE L'IMAGE DE BLOCAGE
            if (blockOverlayImage != null) blockOverlayImage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isPlayerInZone = false;
            SetPlayerAbilities(other.gameObject, true);

            // CACHE L'IMAGE DE BLOCAGE
            if (blockOverlayImage != null) blockOverlayImage.SetActive(false);
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

    private void SetPlayerAbilities(GameObject player, bool state)
    {
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.canThrowGrenade = state;
            controller.canUseParachute = state;
        }
    }

    private void ForceUnlockActions()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) SetPlayerAbilities(player, true);
        
        // CACHE L'IMAGE AU RESPAWN
        if (blockOverlayImage != null) blockOverlayImage.SetActive(false);
        
        _isPlayerInZone = false;
    }
}