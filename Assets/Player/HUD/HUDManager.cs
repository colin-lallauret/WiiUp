using UnityEngine;
using TMPro; // Important pour TextMeshPro
using StarterAssets; // Pour accéder à ton contrôleur

public class HUDManager : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI vitesseText;
    public TextMeshProUGUI hauteurText;
    public TextMeshProUGUI grenadeCountText; // Si tu veux afficher le nombre de grenades

    [Header("Références Joueur")]
    private ThirdPersonController _player;
    private CharacterController _controller;

    void Start()
    {
        // On cherche le joueur dans la scène
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonController>();
        _controller = _player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_player == null) return;

        // 1. Calcul de la vitesse (on prend la vélocité horizontale)
        Vector3 horizontalVelocity = new Vector3(_controller.velocity.x, 0, _controller.velocity.z);
        float speedKmH = horizontalVelocity.magnitude * 3.6f; // Conversion m/s en km/h
        vitesseText.text = Mathf.RoundToInt(speedKmH).ToString() + " km/h";

        // 2. Calcul de la hauteur (altitude par rapport au point 0 ou au sol)
        float altitude = _player.transform.position.y;
        hauteurText.text = Mathf.RoundToInt(altitude).ToString() + " m";
    }
}