using UnityEngine;
using Cinemachine; // Si tu es sur Unity 6 ou version 2023+, utilise : using Unity.Cinemachine;

public class IceZone : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip iceEntrySound;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est bien le joueur qui entre dans la zone
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            
            if (controller != null)
            {
                // 1. Physique : Active la glisse et les visuels de glace aux pieds
                controller.ActivateIce();

                // 2. Visuel : Déclenche le flash "Ice" sur l'HUD via le Singleton
                if (OverlayManager.Instance != null)
                {
                    OverlayManager.Instance.TriggerIceFlash();
                }

                // 3. Audio : Joue le son de congélation (son 2D)
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && iceEntrySound != null)
                {
                    playerAudio.PlayOneShot(iceEntrySound);
                }

                // 4. Caméra : Génère une secousse (Impulse) pour marquer l'impact
                CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
                if (impulse != null)
                {
                    impulse.GenerateImpulse();
                }
            }
        }
    }
}