using UnityEngine;

public class EnemyCarImpact : MonoBehaviour
{
    [Header("Réglages impact")]
    public string playerTag = "Player";   // Tag du joueur
    public float horizontalPower = 8f;
    public float verticalPower = 4f;

    private void OnCollisionEnter(Collision collision)
    {
        Transform root = collision.transform.root;

        if (!root.CompareTag(playerTag))
            return;

        PlayerKnockback knockback = root.GetComponent<PlayerKnockback>();

        if (knockback == null)
        {
            Debug.LogWarning("[EnemyCarImpact] Pas de PlayerKnockback trouvé sur " + root.name);
            return;
        }

        // direction voiture → joueur
        Vector3 dir = (root.position - transform.position).normalized;

        knockback.ApplyKnockback(dir, horizontalPower, verticalPower);

        Debug.Log("[EnemyCarImpact] Knockback appliqué sur " + root.name);
    }
}
