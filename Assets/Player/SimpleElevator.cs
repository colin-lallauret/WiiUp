using UnityEngine;

public class SimpleElevator : MonoBehaviour
{
    [Header("Réglages du mouvement")]
    public float distance = 5f;      // De combien de mètres il monte
    public float vitesse = 1.0f;     // Vitesse du mouvement
    public float pauseTime = 1.0f;   // Temps d'arrêt en haut et en bas

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _timer;
    private bool _movingUp = true;
    private bool _isWaiting = false;

    void Start()
    {
        // On mémorise la position de départ et on calcule la position haute
        _startPos = transform.position;
        _targetPos = _startPos + Vector3.up * distance;
    }

    void Update()
    {
        if (_isWaiting) return;

        // Le timer progresse en fonction de la vitesse
        _timer += Time.deltaTime * vitesse;

        // On calcule la position actuelle de manière fluide
        // Mathf.SmoothStep crée une accélération et une décélération douce
        float progression = Mathf.SmoothStep(0, 1, _timer);

        if (_movingUp)
        {
            transform.position = Vector3.Lerp(_startPos, _targetPos, progression);
        }
        else
        {
            transform.position = Vector3.Lerp(_targetPos, _startPos, progression);
        }

        // Si le mouvement est terminé (timer >= 1)
        if (_timer >= 1.0f)
        {
            StartCoroutine(WaitBeforeTurn());
        }
    }

    System.Collections.IEnumerator WaitBeforeTurn()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(pauseTime);
        
        _timer = 0;             // Reset du timer
        _movingUp = !_movingUp; // On inverse la direction
        _isWaiting = false;
    }
}