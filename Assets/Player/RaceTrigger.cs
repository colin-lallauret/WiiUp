using UnityEngine;

public class RaceTrigger : MonoBehaviour
{
    public enum TriggerType { Start, Finish }
    public TriggerType type;
    
    public TimerManager timerManager;
    public AudioClip triggerSound;
    [Range(0, 1)] public float volume = 1.0f;

    private bool _alreadyActivated = false; // Pour ne l'utiliser qu'une seule fois

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_alreadyActivated)
        {
            // LOGIQUE DE DÉPART
            if (type == TriggerType.Start)
            {
                // On lance le départ (une seule fois grâce à _alreadyActivated)
                if (triggerSound != null) AudioSource.PlayClipAtPoint(triggerSound, transform.position, volume);
                
                timerManager.StartTimer();
                _alreadyActivated = true; 
                Debug.Log("Course commencée ! Ligne de départ désactivée.");
            }
            
            // LOGIQUE D'ARRIVÉE
            else if (type == TriggerType.Finish)
            {
                // ON NE PEUT FINIR QUE SI LA COURSE A ÉTÉ LANCÉE
                if (timerManager.RaceInProgress)
                {
                    if (triggerSound != null) AudioSource.PlayClipAtPoint(triggerSound, transform.position, volume);
                    
                    timerManager.StopTimer();
                    _alreadyActivated = true; 
                    Debug.Log("Course terminée ! Ligne d'arrivée désactivée.");
                }
                else
                {
                    Debug.Log("Tu n'as pas encore passé la ligne de départ !");
                }
            }
        }
    }
}