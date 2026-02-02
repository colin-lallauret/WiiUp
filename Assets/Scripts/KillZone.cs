using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private string tagToKill = "EnemyCar";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[KillZone] Trigger enter avec: {other.name}, tag = {other.tag}");

        if (other.CompareTag(tagToKill))
        {
            Debug.Log($"[KillZone] Destruction de: {other.name}");
            Destroy(other.gameObject);
        }
    }
}
