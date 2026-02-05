using UnityEngine;
using Cinemachine; // Ou Unity.Cinemachine

public class ClearEffect : MonoBehaviour
{
    public AudioClip clearSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                // 1. Physique : Reset des états
                controller.ClearIce();
                controller.ClearSpace();

                // 2. Visuel : Flash de nettoyage
                if (OverlayManager.Instance != null)
                {
                    OverlayManager.Instance.TriggerClearFlash();
                }

                // 3. Audio : Son de reset
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && clearSound != null)
                    playerAudio.PlayOneShot(clearSound);

                // 4. Caméra : Petit coup sec pour le feeling
                CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
                if (impulse != null) impulse.GenerateImpulse();
            }
        }
    }
}