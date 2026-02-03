using UnityEngine;
using StarterAssets;

public class GrenadeLauncher : MonoBehaviour
{
    [Header("Références")]
    public GameObject grenadePrefab;
    public Transform spawnPoint;

    [Header("Réglages")]
    public float throwForce = 20f;
    public float upwardForce = 2f;
    public float animationDelay = 0.55f; // Délai de synchronisation avec ton animation
    public float throwCooldown = 2.0f;   // Temps de recharge entre deux tirs

    private StarterAssetsInputs _input;
    private ThirdPersonController _controller; // Référence au contrôleur
    private Camera _mainCamera;
    private Animator _animator;
    private int _animIDThrow;
    
    private float _nextThrowTime = 0f; 

    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _controller = GetComponent<ThirdPersonController>(); // On récupère le script de mouvement
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _animIDThrow = Animator.StringToHash("Throw");
    }

    void Update()
    {
        // On vérifie : 
        // 1. Si le joueur appuie sur le bouton grenade
        // 2. Si le cooldown est terminé
        // 3. SI LE PERSONNAGE N'EST PAS EN TRAIN DE PLANER
        if (_input.grenade && Time.time >= _nextThrowTime && !_controller.IsGliding)
        {
            // On définit quand sera le prochain tir autorisé
            _nextThrowTime = Time.time + throwCooldown;

            // 1. On lance l'animation de lancer
            _animator.SetTrigger(_animIDThrow);
            
            // 2. On lance la grenade avec le délai de 0.55s
            Invoke("Throw", animationDelay);
            
            _input.grenade = false; 
        }
        else if (_input.grenade)
        {
            // Si le joueur clique mais que le cooldown n'est pas fini OU qu'il plane
            // On reset l'input pour éviter un tir automatique non voulu
            _input.grenade = false;
        }
    }

    void Throw()
    {
        // On revérifie au moment fatidique (après le délai d'animation) 
        // au cas où le joueur aurait ouvert son parapluie entre-temps
        if (grenadePrefab == null || spawnPoint == null || _controller.IsGliding) return;

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