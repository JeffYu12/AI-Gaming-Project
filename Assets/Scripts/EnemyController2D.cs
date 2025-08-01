using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController2D : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRadius = 5f;

    private Transform player;
    private Rigidbody2D rb;
    private Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        var p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null || health == null) return;
        if (health.currentHealth <= 0) return; // dead, stop moving

        if (Vector2.Distance(rb.position, player.position) <= detectionRadius)
        {
            Vector2 dir = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Optional: for custom death handling instead of Health.Die()
    public void OnDeath()
    {
        // e.g., play VFX, drop loot, increment score
        Destroy(gameObject);
    }
}

