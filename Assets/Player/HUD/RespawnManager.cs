using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class RespawnManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject barParent; 
    public Image fillImage;      

    [Header("Settings")]
    public float holdTime = 3f;  
    
    private float _timer = 0f;
    private StarterAssetsInputs _input;
    private ThirdPersonController _player;

    void Start()
    {
        // On récupère les composants du joueur
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            _player = playerObj.GetComponent<ThirdPersonController>();
            _input = playerObj.GetComponent<StarterAssetsInputs>();
        }
        
        fillImage.fillAmount = 0;
        barParent.SetActive(false);
    }

    void Update()
    {
        if (_input == null || _player == null) return;

        // Si la touche R est maintenue
        if (_input.respawn)
        {
            // 1. On affiche la barre
            if (!barParent.activeSelf) barParent.SetActive(true);

            // 2. On augmente le timer
            _timer += Time.deltaTime;
            fillImage.fillAmount = _timer / holdTime;

            // 3. Si on atteint les 3 secondes
            if (_timer >= holdTime)
            {
                _player.Respawn(); // C'est ici que l'ordre est donné !
                ResetRespawnLogic();
                _input.respawn = false; // On force l'arrêt pour éviter un double respawn
            }
        }
        else
        {
            // Si on relâche la touche avant la fin, on reset tout
            if (_timer > 0) ResetRespawnLogic();
        }
    }

    void ResetRespawnLogic()
    {
        _timer = 0f;
        fillImage.fillAmount = 0;
        barParent.SetActive(false);
    }
}