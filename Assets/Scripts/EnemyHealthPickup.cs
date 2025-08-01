using UnityEngine;

public class EnemyHealthPickup : MonoBehaviour
{
    public int healAmount = 30;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
        // Check if the collider is an enemy with Health
        var enemyHealth = other.GetComponent<Health>();
        if (enemyHealth != null && other.CompareTag("Enemy"))
        {
            enemyHealth.currentHealth = Mathf.Min(enemyHealth.maxHealth, enemyHealth.currentHealth + healAmount);
            Destroy(gameObject);
        }
    }
}

