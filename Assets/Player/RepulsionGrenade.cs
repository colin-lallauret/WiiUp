using UnityEngine;
using System.Collections;
using StarterAssets;
using Cinemachine; 

[RequireComponent(typeof(AudioSource))]
public class RepulsionGrenade : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 5.0f;         
    public float explosionForce = 15f;  
    public float fuseTime = 1.5f;       

    [Header("Super Jump Settings")]
    public float verticalBoost = 12f; 

    [Header("Visual & Audio Effects")]
    public AudioClip startSound;        // Le son qui commence au toucher (mèche/bip unique)
    public Color flashColor = new Color(1f, 0f, 1f, 1f); 
    public GameObject explosionEffect; 

    private bool _isSequenceStarted = false; 
    private Rigidbody _rb;
    private Renderer _renderer;
    private Color _originalColor;
    private AudioSource _audioSource;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_renderer != null) _originalColor = _renderer.material.color;
        if (_rb != null) _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1.0f; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isSequenceStarted) return;
        _isSequenceStarted = true;

        _rb.linearDamping = 10f; 
        _rb.angularDamping = 10f;
        
        ContactPoint contact = collision.contacts[0];
        transform.position = contact.point + contact.normal * 0.08f;

        if (collision.gameObject.GetComponent<SimpleElevator>() != null)
            transform.parent = collision.transform;

        // --- 1. LE SON SE JOUE UNE SEULE FOIS ICI (À L'IMPACT) ---
        if (startSound != null && _audioSource != null) 
        {
            _audioSource.PlayOneShot(startSound);
        }

        // --- 2. ON LANCE LA SÉQUENCE VISUELLE ---
        StartCoroutine(ExplosionSequence());
    }

    IEnumerator ExplosionSequence()
    {
        // On ne gère plus que le visuel ici
        int numberOfFlashes = 4;
        float interval = fuseTime / numberOfFlashes;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            if (_renderer != null) _renderer.material.color = flashColor;
            yield return new WaitForSeconds(0.05f); 
            if (_renderer != null) _renderer.material.color = _originalColor;

            yield return new WaitForSeconds(interval - 0.05f);
        }

        Explode();
    }

    void Explode()
    {
        var impulse = GetComponent<CinemachineCollisionImpulseSource>();
        if (impulse != null) impulse.GenerateImpulse();

        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            ThirdPersonController player = hit.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                Vector3 horizontalDir = (hit.transform.position - transform.position);
                horizontalDir.y = 0; 
                Vector3 finalLaunchVector = horizontalDir.normalized * (explosionForce * 0.5f);
                finalLaunchVector.y = verticalBoost; 

                player.LaunchPlayer(finalLaunchVector);
            }
        }

        StartCoroutine(CleanUpRoutine());
    }

    IEnumerator CleanUpRoutine()
    {
        if (_renderer != null) _renderer.enabled = false;
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        if (_rb != null) _rb.isKinematic = true;

        // On attend la fin du son de l'explosion
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}