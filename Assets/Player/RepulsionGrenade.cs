using UnityEngine;
using System.Collections;
using StarterAssets;

public class RepulsionGrenade : MonoBehaviour
{
    public float radius = 5.0f;
    public float explosionForce = 15f;
    public float fuseTime = 3.0f;

    private bool _hasCollided = false;
    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasCollided)
        {
            _hasCollided = true;

            // --- LA MAGIE POUR COLLER ---
            // 1. On fige la grenade en position et rotation
            _rb.isKinematic = true; 
            
            // 2. On la parente à l'objet touché (optionnel, pour qu'elle bouge avec un mur mobile)
            transform.parent = collision.transform;

            // Lancer le compte à rebours
            StartCoroutine(ExplosionTimer());
        }
    }

    IEnumerator ExplosionTimer()
    {
        // On peut ajouter un petit son de "Bip" ici
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            ThirdPersonController player = hit.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                Vector3 dir = hit.transform.position - transform.position;
                // Propulsion avec un boost vertical
                player.LaunchPlayer((dir.normalized + Vector3.up).normalized * explosionForce);
            }
        }
        
        // Détruire la grenade
        Destroy(gameObject);
    }
}