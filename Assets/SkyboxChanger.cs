using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [System.Serializable]
    public struct SkyboxStep
    {
        public float heightThreshold; // La hauteur à atteindre
        public Material skyboxMaterial; // Le ciel correspondant
    }

    [Header("Réglages")]
    public Transform playerTransform; // Glisse Miaxo ici
    public SkyboxStep[] skyboxSteps; // Liste de tes ciels par palier
    public float changeSmoothing = 0.1f; // Douceur de la transition (optionnel)

    private Material _currentSkybox;

    void Update()
    {
        if (playerTransform == null || skyboxSteps.Length == 0) return;

        float playerY = playerTransform.position.y;
        Material targetMaterial = skyboxSteps[0].skyboxMaterial;

        // On parcourt les paliers pour trouver le plus haut atteint
        for (int i = 0; i < skyboxSteps.Length; i++)
        {
            if (playerY >= skyboxSteps[i].heightThreshold)
            {
                targetMaterial = skyboxSteps[i].skyboxMaterial;
            }
        }

        // Si le ciel doit changer, on l'applique
        if (RenderSettings.skybox != targetMaterial)
        {
            RenderSettings.skybox = targetMaterial;
            
            // Force Unity à mettre à jour les reflets de la scène
            DynamicGI.UpdateEnvironment();
        }
    }
}