using UnityEngine;
using System.Collections;

public class ItemCollect : MonoBehaviour
{
    [Header("Son")]
    public AudioClip collectSound;
    [Range(0, 1)] public float volume = 1.0f;

    [Header("Secousse Caméra (Shake)")]
    public float duration = 0.15f;  // Durée de la secousse
    public float magnitude = 0.2f; // Intensité (0.2 est une bonne base)

    private bool _isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isCollected)
        {
            _isCollected = true;

            // 1. Jouer le son
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);

            // 2. Déclencher le Shake (on cherche la caméra principale)
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                // On lance la secousse via une petite coroutine simple
                StartCoroutine(ShakeCamera(mainCam.transform));
            }

            // 3. Cacher l'objet visuellement (pour que le shake soit visible avant la destruction)
            GetComponent<MeshRenderer>().enabled = false;
            
            // 4. Détruire l'objet après un petit délai
            Destroy(gameObject, duration + 0.1f);
        }
    }

    private IEnumerator ShakeCamera(Transform camTransform)
    {
        Vector3 originalPos = camTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // On calcule un décalage aléatoire
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camTransform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null; // Attendre la frame suivante
        }

        // On remet la caméra à sa position d'origine
        camTransform.localPosition = originalPos;
    }
}