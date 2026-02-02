using UnityEngine;
using System.Collections;
using StarterAssets; // important pour accéder à ThirdPersonController

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ThirdPersonController))]
public class PlayerKnockback : MonoBehaviour
{
    [Header("Réglages knockback")]
    public float gravity = -25f;          // gravité appliquée pendant le knockback
    public float horizontalDrag = 3f;     // frein horizontal
    public float minSpeedToStop = 0.5f;   // quand on passe en dessous, on arrête le knockback

    private CharacterController _controller;
    private ThirdPersonController _tpc;
    private bool _isKnockback = false;
    private Vector3 _velocity;            // vitesse actuelle du knockback

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _tpc = GetComponent<ThirdPersonController>();
    }

    /// <summary>
    /// Appelé par la voiture pour propulser le joueur
    /// </summary>
    public void ApplyKnockback(Vector3 direction, float horizontalPower, float verticalPower)
    {
        direction.y = 0f;
        direction.Normalize();

        // Vitesse initiale du knockback
        _velocity = direction * horizontalPower + Vector3.up * verticalPower;

        if (!_isKnockback)
        {
            StartCoroutine(KnockbackRoutine());
        }
    }

    private IEnumerator KnockbackRoutine()
    {
        _isKnockback = true;

        // On coupe temporairement les contrôles du ThirdPersonController
        _tpc.enabled = false;

        while (true)
        {
            float dt = Time.deltaTime;

            // Gravité sur l'axe Y
            _velocity.y += gravity * dt;

            // Frein sur l'horizontale (X/Z)
            Vector3 horizontal = new Vector3(_velocity.x, 0f, _velocity.z);
            horizontal = Vector3.Lerp(horizontal, Vector3.zero, horizontalDrag * dt);

            _velocity.x = horizontal.x;
            _velocity.z = horizontal.z;

            // Déplacement du joueur
            _controller.Move(_velocity * dt);

            // Si la vitesse est devenue très faible → on arrête
            Vector3 flatVel = new Vector3(_velocity.x, 0f, _velocity.z);
            if (flatVel.magnitude < minSpeedToStop && _controller.isGrounded && _velocity.y <= 0f)
            {
                break;
            }

            yield return null;
        }

        // On rend le contrôle au ThirdPersonController
        _tpc.enabled = true;
        _isKnockback = false;
    }
}
