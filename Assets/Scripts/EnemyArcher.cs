using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyArcher : MonoBehaviour
{
    public enum State { Idle, Approaching, Attacking, Retreating }

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float minDistance = 3f;       // retreat below this
    public float idealDistance = 6f;     // preferred fighting range
    public float detectionDistance = 10f;
    public float retreatSpeedMultiplier = 1.3f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireInterval = 1.5f;
    public float projectileSpeed = 12f;
    public int damage = 20;

    private Transform player;
    private Rigidbody2D rb;
    private float fireTimer;
    private State currentState = State.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;

        fireTimer = 0f;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // ?? FSM transition logic
        if (distance > detectionDistance)
            currentState = State.Idle;
        else if (distance > idealDistance)
            currentState = State.Approaching;
        else if (distance < minDistance)
            currentState = State.Retreating;
        else
            currentState = State.Attacking;

        fireTimer -= Time.deltaTime;

        // ?? FSM behavior
        switch (currentState)
        {
            case State.Idle:
                break;

            case State.Approaching:
                MoveTowardPlayer();
                break;

            case State.Retreating:
                MoveAwayFromPlayer();
                break;

            case State.Attacking:
                ShootIfReady();
                break;
        }

        FacePlayer();
    }

    void MoveTowardPlayer()
    {
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 dir = ((Vector2)transform.position - (Vector2)player.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * retreatSpeedMultiplier * Time.deltaTime);
    }

    void ShootIfReady()
    {
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireInterval;
        }
    }

    void Shoot()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
        Vector3 spawnPos = firePoint.position + (Vector3)direction * 0.2f;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        var rbProj = proj.GetComponent<Rigidbody2D>();
        if (rbProj != null)
            rbProj.linearVelocity = direction * projectileSpeed;

        var script = proj.GetComponent<EnemyProjectile>();
        if (script == null)
            script = proj.AddComponent<EnemyProjectile>();

        script.damage = damage;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void FacePlayer()
    {
        Vector2 aimDir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, idealDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}



