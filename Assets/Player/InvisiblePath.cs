using UnityEngine;

public class InvisiblePath : MonoBehaviour
{
    [Tooltip("Si coché, l'objet devient invisible dès le lancement du jeu")]
    public bool invisibleOnStart = true;

    private MeshRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();

        if (invisibleOnStart && _renderer != null)
        {
            _renderer.enabled = false; // On cache le visuel
            // Le Collider reste actif par défaut car on ne le touche pas
        }
    }

    // Fonction pour rendre le chemin visible (si tu en as besoin plus tard)
    public void RevealPath(bool state)
    {
        if (_renderer != null) _renderer.enabled = state;
    }
}