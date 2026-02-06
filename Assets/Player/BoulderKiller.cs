using UnityEngine;

public class BoulderKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si l'objet qui entre a le tag "Obstacle"
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            // Debug.Log("Cube supprimé par le BoulderKiller"); // Optionnel pour vérifier
        }
        
        // Sécurité supplémentaire : si un cube n'a pas de tag mais est un clone
        else if (other.name.Contains("(Clone)"))
        {
            Destroy(other.gameObject);
        }
    }
}