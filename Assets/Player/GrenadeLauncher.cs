using UnityEngine;
using StarterAssets;
using System.Collections;

public class GrenadeLauncher : MonoBehaviour
{
    [Header("Références")]
    public GameObject grenadePrefab; // GrenadePropu
    public Transform spawnPoint;    // spawnpoint

    [Header("Réglages")]
    public float throwForce = 15f;       // Valeur de ton image
    public float upwardForce = 2f;       // Valeur de ton image
    public float animationDelay = 0.55f; // Valeur de ton image
    public float throwCooldown = 5.0f;   // Valeur de ton image

    [Header("Audio")]
    public AudioClip throwSound;         // throw
    public float audioDelay = 0.5f;      // Valeur de ton image

    private StarterAssetsInputs _input;
    private ThirdPersonController _controller; 
    private Camera _mainCamera;
    private Animator _animator;
    private AudioSource _audioSource; 
    private int _animIDThrow;
    private float _nextThrowTime = 0f; 

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _controller = GetComponent<ThirdPersonController>(); 
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>(); 
        _animIDThrow = Animator.StringToHash("Throw");
    }

    void Update()
    {
        if (_input.grenade && Time.time >= _nextThrowTime && !_controller.IsGliding && _controller.canThrowGrenade)
        {
            _nextThrowTime = Time.time + throwCooldown;
            StartCoroutine(PlayThrowSoundDelayed());
            if (_animator != null) _animator.SetTrigger(_animIDThrow);
            Invoke("Throw", animationDelay);
            _input.grenade = false; 
        }
        else if (_input.grenade) { _input.grenade = false; }
    }

    IEnumerator PlayThrowSoundDelayed()
    {
        yield return new WaitForSeconds(audioDelay);
        if (_audioSource != null && throwSound != null) _audioSource.PlayOneShot(throwSound);
    }

    void Throw()
    {
        if (grenadePrefab == null || spawnPoint == null || _controller.IsGliding || !_controller.canThrowGrenade) return;
        GameObject g = Instantiate(grenadePrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = g.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 forceDirection = _mainCamera.transform.forward;
            Vector3 finalForce = (forceDirection * throwForce) + (Vector3.up * upwardForce);
            rb.AddForce(finalForce, ForceMode.Impulse);
        }
    }
}