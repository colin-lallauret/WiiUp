using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float _timeElapsed;
    private bool _isTimerRunning = false;
    
    // Cette variable permet de savoir si on a passé le départ
    public bool RaceInProgress { get; private set; }

    void Update()
    {
        if (_isTimerRunning)
        {
            _timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        _timeElapsed = 0;
        _isTimerRunning = true;
        RaceInProgress = true; // La course commence
    }

    public void StopTimer()
    {
        _isTimerRunning = false;
        RaceInProgress = false; // La course est finie
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_timeElapsed / 60);
        int seconds = Mathf.FloorToInt(_timeElapsed % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}