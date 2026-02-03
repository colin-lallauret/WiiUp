using UnityEngine;
using System.Collections;
using StarterAssets;
using Cinemachine; // Utilise "using Unity.Cinemachine;" si tu es sur la toute dernière version

public class RepulsionGrenade : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 5.0f;
    public float explosionForce = 15f;
    public float fuseTime = 3.0f;

    [Header("Visual & Audio Effects")]
    public AudioClip bipSound;      
    public Color flashColor = Color.white; 
    public GameObject explosionEffect; 

    private bool _isSequenceStarted = false; 
    private Rigidbody _rb;
    private Renderer _renderer;
    private Color _originalColor;

    void Awake()
    {
        // --- SÉCURITÉ ANTI-DOUBLE SPAWN ---
        Collider[] alreadyThere = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var c in alreadyThere)
        {
            if (c.gameObject != this.gameObject && c.GetComponent<RepulsionGrenade>())
            {
                Destroy(this.gameObject); 
                return;
            }
        }

        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        if (_renderer != null) _originalColor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isSequenceStarted) return;
        _isSequenceStarted = true;

        _rb.isKinematic = true; 
        transform.parent = collision.transform;

        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        yield return new WaitForFixedUpdate();

        float timer = 0;
        while (timer < fuseTime)
        {
            // Verrou pour le nom du son
            if (GameObject.Find("GRENADE_BIP_SOUND") == null)
            {
                if (bipSound != null)
                {
                    AudioSource.PlayClipAtPoint(bipSound, transform.position);
                    GameObject soundObj = GameObject.Find("One shot audio");
                    if (soundObj != null) soundObj.name = "GRENADE_BIP_SOUND";
                }
            }

            if (_renderer != null) _renderer.material.color = flashColor;
            yield return new WaitForSeconds(0.05f); 
            if (_renderer != null) _renderer.material.color = _originalColor;

            float waitTime = (timer > fuseTime * 0.7f) ? 0.25f : 0.5f;
            yield return new WaitForSeconds(waitTime);
            timer += (waitTime + 0.05f);
        }

        Explode();
    }

    void Explode()
    {
        // --- DÉCLENCHEMENT DU SHAKE ---
        var impulse = GetComponent<CinemachineCollisionImpulseSource>();
        if (impulse != null)
        {
            Debug.Log("BOUM : Impulsion envoyée !");
            impulse.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("Composant Impulse manquant sur le Prefab !");
        }

        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            ThirdPersonController player = hit.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                player.LaunchPlayer((dir + Vector3.up * 1.5f).normalized * explosionForce);
            }
        }
        Destroy(gameObject);
    }
}