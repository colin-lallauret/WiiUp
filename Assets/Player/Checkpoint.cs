using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip checkpointSound; 

    private bool _isActivated = false; // Verrou pour ne jouer le son qu'une fois

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est le joueur ET si ce checkpoint n'a pas encore servi
        if (!_isActivated && other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                // 1. On définit la nouvelle position de respawn
                controller.SetCheckpoint(transform.position);
                
                // 2. On verrouille ce checkpoint pour le son
                _isActivated = true;

                // 3. On joue le son en 2D via l'AudioSource du joueur
                AudioSource playerAudio = other.GetComponent<AudioSource>();
                if (playerAudio != null && checkpointSound != null)
                {
                    playerAudio.PlayOneShot(checkpointSound);
                }

                Debug.Log("Nouveau Checkpoint validé !");
            }
        }
    }
}