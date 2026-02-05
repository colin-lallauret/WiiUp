using UnityEngine;
using Cinemachine; // Ou Unity.Cinemachine

public class SpaceZone : MonoBehaviour
{
    public AudioClip spaceEntrySound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                controller.ActivateSpace();

                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && spaceEntrySound != null)
                    playerAudio.PlayOneShot(spaceEntrySound);

                CinemachineImpulseSource impulse = GetComponent<CinemachineImpulseSource>();
                if (impulse != null) impulse.GenerateImpulse();
            }
        }
    }
}