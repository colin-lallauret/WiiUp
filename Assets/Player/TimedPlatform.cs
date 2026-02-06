using UnityEngine;
using System.Collections;

public class TimedPlatform : MonoBehaviour
{
    [Header("Réglages du Rythme")]
    public float timeActive = 2.0f;   // Temps où l'objet est présent
    public float timeInvisible = 2.0f; // Temps où l'objet est disparu
    public float startOffset = 0f;    // Décalage pour le rythme "Une-Deux"

    private MeshRenderer _renderer;
    private Collider _collider;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();

        // Lancement de la boucle infinie
        StartCoroutine(PlatformCycle());
    }

    IEnumerator PlatformCycle()
    {
        // Attendre le décalage initial (pour désynchroniser les plateformes)
        yield return new WaitForSeconds(startOffset);

        while (true)
        {
            // ÉTAT : APPARAÎT
            SetPlatformState(true);
            yield return new WaitForSeconds(timeActive);

            // ÉTAT : DISPARAÎT
            SetPlatformState(false);
            yield return new WaitForSeconds(timeInvisible);
        }
    }

    void SetPlatformState(bool state)
    {
        if (_renderer) _renderer.enabled = state;
        if (_collider) _collider.enabled = state;
        
        // Optionnel : Si tu as des enfants (visuels), tu peux utiliser :
        // gameObject.SetActive(state); 
        // Mais attention, SetActive(false) arrête le script sur l'objet !
    }
}