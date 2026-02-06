using UnityEngine;
using StarterAssets;
using System.Collections;

public class GrenadeLauncher : MonoBehaviour
{
    [Header("Références")]
    public GameObject grenadePrefab;
    public Transform spawnPoint;

    [Header("Réglages")]
    public float throwForce = 20f;
    public float upwardForce = 2f;
    public float animationDelay = 0.55f; // Délai pour l'apparition de la grenade
    public float throwCooldown = 2.0f;   // Temps de recharge entre deux tirs

    [Header("Audio")]
    public AudioClip throwSound; 
    public float audioDelay = 0.2f; // Petit délai avant de jouer le son

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
        // AJOUT : On vérifie si canThrowGrenade est vrai avant de lancer
        if (_input.grenade && Time.time >= _nextThrowTime && !_controller.IsGliding && _controller.canThrowGrenade)
        {
            _nextThrowTime = Time.time + throwCooldown;

            // 1. Lance la Coroutine pour le son avec un délai
            StartCoroutine(PlayThrowSoundDelayed());

            // 2. On lance l'animation de lancer
            _animator.SetTrigger(_animIDThrow);
            
            // 3. On lance la grenade physiquement après le délai d'animation
            Invoke("Throw", animationDelay);
            
            _input.grenade = false; 
        }
        else if (_input.grenade)
        {
            // On reset l'input même si le tir est bloqué pour éviter les tirs automatiques en sortant de zone
            _input.grenade = false;
        }
    }

    // Fonction spéciale pour attendre avant de jouer le son
    IEnumerator PlayThrowSoundDelayed()
    {
        yield return new WaitForSeconds(audioDelay);

        if (_audioSource != null && throwSound != null)
        {
            _audioSource.PlayOneShot(throwSound);
        }
    }

    void Throw()
    {
        // On vérifie une dernière fois les conditions de sécurité
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