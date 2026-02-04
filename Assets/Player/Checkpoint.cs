using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est le joueur qui touche l'objet
        if (other.CompareTag("Player"))
        {
            // On récupère le script de gestion du joueur
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                // On met à jour la position de respawn du joueur
                controller.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint activé à : " + transform.position);
            }
        }
    }
}