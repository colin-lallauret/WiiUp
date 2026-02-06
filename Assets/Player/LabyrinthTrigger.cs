using UnityEngine;
using StarterAssets;
using System.Collections;

public class LabyrinthTrigger : MonoBehaviour
{
    [Header("Réglages du Cycle")]
    public float invisibleDuration = 10f; 
    public float revealDuration = 1f;    

    [Header("Audio")]
    public AudioClip magicEntrySound; 
    private AudioSource _audioSource;
    private bool _hasPlayedSound = false; // Sécurité pour ne jouer le son qu'une fois

    private Coroutine _cycleCoroutine;
    private bool _isPlayerInside = false;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _audioSource = player.GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        ThirdPersonController.OnPlayerRespawnEvent += ResetLabyrinth;
    }

    private void OnDisable()
    {
        ThirdPersonController.OnPlayerRespawnEvent -= ResetLabyrinth;
    }

    private void ResetLabyrinth()
    {
        StopLabyrinthCycle();
        ShowLabyrinth(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;

            // JOUE LE SON MAGIC SEULEMENT LA PREMIÈRE FOIS
            if (_audioSource != null && magicEntrySound != null && !_hasPlayedSound)
            {
                _audioSource.PlayOneShot(magicEntrySound);
                _hasPlayedSound = true; // On verrouille le son pour toujours
            }

            _cycleCoroutine = StartCoroutine(LabyrinthCycleRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopLabyrinthCycle();
        }
    }

    private void StopLabyrinthCycle()
    {
        _isPlayerInside = false;
        if (_cycleCoroutine != null) StopCoroutine(_cycleCoroutine);
        ShowLabyrinth(true); 
    }

    IEnumerator LabyrinthCycleRoutine()
    {
        while (_isPlayerInside)
        {
            ShowLabyrinth(false);
            yield return new WaitForSeconds(invisibleDuration);

            ShowLabyrinth(true);
            yield return new WaitForSeconds(revealDuration);
        }
    }

    private void ShowLabyrinth(bool state)
    {
        InvisiblePath[] paths = FindObjectsOfType<InvisiblePath>();
        foreach (InvisiblePath path in paths)
        {
            path.RevealPath(state);
        }
    }
}