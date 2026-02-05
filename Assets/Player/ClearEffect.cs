using UnityEngine;

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
                controller.ClearIce();   // Enlève la glace
                controller.ClearSpace(); // Enlève la gravité spatiale

                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && clearSound != null)
                    playerAudio.PlayOneShot(clearSound);
            }
        }
    }
}