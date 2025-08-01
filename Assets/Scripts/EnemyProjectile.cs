using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 20;
    public float lifetime = 4f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var h = collision.gameObject.GetComponent<Health>();
        if (h != null)
        {
            h.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

