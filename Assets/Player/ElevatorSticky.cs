using UnityEngine;

public class ElevatorSticky : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Le joueur devient un enfant de l'ascenseur
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Le joueur redevient indépendant
            other.transform.SetParent(null);
        }
    }
}