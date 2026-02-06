using UnityEngine;
using StarterAssets;

public class ZoneWithOutTools : MonoBehaviour
{
    [Header("UI Feedback")]
    public GameObject blockOverlayImage; // Glisse ton image "BlockOverlay" ici

    private void OnEnable()
    {
        // Sécurité : Si on meurt dans la zone, on débloque tout au respawn
        ThirdPersonController.OnPlayerRespawnEvent += ForceUnlock;
    }

    private void OnDisable()
    {
        ThirdPersonController.OnPlayerRespawnEvent -= ForceUnlock;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetToolsEnabled(other.gameObject, false);
            if (blockOverlayImage != null) blockOverlayImage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetToolsEnabled(other.gameObject, true);
            if (blockOverlayImage != null) blockOverlayImage.SetActive(false);
        }
    }

    private void SetToolsEnabled(GameObject player, bool state)
    {
        ThirdPersonController controller = player.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.canThrowGrenade = state;
            controller.canUseParachute = state;
        }
    }

    private void ForceUnlock()
    {
        // On récupère le joueur et on force la réactivation
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) SetToolsEnabled(player, true);
        
        // On cache l'image de blocage
        if (blockOverlayImage != null) blockOverlayImage.SetActive(false);
    }
}