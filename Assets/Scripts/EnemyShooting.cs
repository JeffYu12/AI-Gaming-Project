using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShooting : MonoBehaviour
{
    public float detectionRadius = 8f;
    public float fireInterval = 1.5f; // seconds between shots
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 12f;
    public int damage = 20;

    private Transform player;
    private float fireTimer;

    void Start()
    {
        var p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
        fireTimer = 0f;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > detectionRadius) return;

        // aim direction
        Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        // optionally rotate enemy to face player (if you have a facing sprite)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // adjust if sprite orientation differs

        // handle firing
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire(dir);
            fireTimer = fireInterval;
        }
    }

    void Fire(Vector2 direction)
    {
        Vector3 spawnPos = firePoint.position + (Vector3)direction * 0.2f; // avoid overlap
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        var rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        // attach a small bullet script to deal damage
        var bs = proj.GetComponent<EnemyProjectile>();
        if (bs == null)
        {
            bs = proj.AddComponent<EnemyProjectile>();
            bs.damage = damage;
        }

        // rotate projectile to face travel direction if desired
        float projAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0f, 0f, projAngle - 90f);
    }
}

