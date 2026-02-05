using UnityEngine;
using Cinemachine; // Si erreur ici, utilise : using Unity.Cinemachine;

public class SpaceZone : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip spaceEntrySound;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est bien le joueur
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            
            if (controller != null)
            {
                // 1. Applique la physique
                controller.ActivateSpace();

                // 2. Déclenche le flash visuel (HUD)
                if (OverlayManager.Instance != null)
                {
                    OverlayManager.Instance.TriggerSpaceFlash();
                }

                // 3. Joue le son spatial
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && spaceEntrySound != null)
                {
                    playerAudio.PlayOneShot(spaceEntrySound);
                }

                // 4. Secousse caméra
                CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
                if (impulse != null)
                {
                    impulse.GenerateImpulse();
                }
            }
        }
    }
}