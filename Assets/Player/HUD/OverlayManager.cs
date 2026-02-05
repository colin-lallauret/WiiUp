using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance;

    public Image iceOverlay;
    public Image spaceOverlay;
    public Image clearOverlay; // Glisse une image blanche ici (Alpha 0 au départ)

    public float flashDuration = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerIceFlash() { StartCoroutine(FlashRoutine(iceOverlay)); }
    public void TriggerSpaceFlash() { StartCoroutine(FlashRoutine(spaceOverlay)); }
    
    // Nouvelle fonction pour le Clear
    public void TriggerClearFlash() 
    { 
        StartCoroutine(FlashRoutine(clearOverlay)); 
    }

    IEnumerator FlashRoutine(Image img)
    {
        if (img == null) yield break;

        float elapsed = 0f;
        float halfDuration = flashDuration / 2f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / halfDuration);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / halfDuration);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
    }
}