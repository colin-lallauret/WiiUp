using UnityEngine;
using StarterAssets;

public class ZoneWithOutTools : MonoBehaviour
{
    [Header("UI Feedback")]
    public GameObject blockOverlayImage;

    private void OnEnable() { ThirdPersonController.OnPlayerRespawnEvent += ForceUnlock; }
    private void OnDisable() { ThirdPersonController.OnPlayerRespawnEvent -= ForceUnlock; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BoulderSpawner.globalZoneCount++; // Utilise le même compteur que le spawner
            UpdateState(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BoulderSpawner.globalZoneCount--;
            if (BoulderSpawner.globalZoneCount < 0) BoulderSpawner.globalZoneCount = 0;
            UpdateState(other.gameObject);
        }
    }

    private void UpdateState(GameObject player)
    {
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            bool shouldBeEnabled = (BoulderSpawner.globalZoneCount == 0);
            controller.canThrowGrenade = shouldBeEnabled;
            controller.canUseParachute = shouldBeEnabled;
            if (blockOverlayImage != null) blockOverlayImage.SetActive(!shouldBeEnabled);
        }
    }

    private void ForceUnlock()
    {
        BoulderSpawner.globalZoneCount = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) UpdateState(player);
    }
}