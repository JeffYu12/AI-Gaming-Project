using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Health, Score }
    public ItemType itemType;
    public int value = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (itemType == ItemType.Health)
            {
                var health = other.GetComponent<Health>();
                if (health != null)
                    health.currentHealth = Mathf.Min(health.maxHealth, health.currentHealth + value);
            }
            // add score logic here
            Destroy(gameObject);
        }
    }
}
