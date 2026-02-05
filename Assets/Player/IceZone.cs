using UnityEngine;
using Cinemachine; // <--- C'est souvent juste Cinemachine sans "Unity." devant

public class IceZone : MonoBehaviour
{
    public float iceDuration = 5.0f;
    public AudioClip iceEntrySound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                controller.ActivateIce(iceDuration);

                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && iceEntrySound != null)
                {
                    playerAudio.PlayOneShot(iceEntrySound);
                }

                // Utilisation du composant Impulse pour le shake
                CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
                if (impulse != null)
                {
                    impulse.GenerateImpulse();
                }
            }
        }
    }
}