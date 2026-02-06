using UnityEngine;
using StarterAssets;

public class CubeImpact : MonoBehaviour
{
    public float repulsionForce = 20f; // Force de l'éjection

    private void OnCollisionEnter(Collision collision)
    {
        // On vérifie si on touche le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            ThirdPersonController controller = collision.gameObject.GetComponent<ThirdPersonController>();
            
            if (controller != null)
            {
                // On calcule la direction de l'impact (du cube vers le joueur)
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                
                // On ajoute une force vers le haut pour l'effet d'éjection
                pushDirection.y = 0.5f; 

                // On utilise ta fonction LaunchPlayer déjà présente dans ton script !
                controller.LaunchPlayer(pushDirection.normalized * repulsionForce);
            }
            
            // Optionnel : Détruire le cube après l'impact pour ne pas encombrer
            Destroy(gameObject, 2f);
        }
    }
}