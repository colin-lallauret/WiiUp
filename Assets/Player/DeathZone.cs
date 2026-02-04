using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si l'objet qui entre a le tag "NeedRespawn" 
        // OU si c'est le joueur qui touche une zone de mort
        if (other.CompareTag("Player"))
        {
            var controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller != null)
            {
                // On appelle la fonction Respawn que nous avons créée dans le controller
                controller.Respawn();
                Debug.Log("Le joueur est tombé ! Respawn automatique.");
            }
        }
    }
}