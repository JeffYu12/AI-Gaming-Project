using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWarrior : MonoBehaviour
{
    public enum State { Idle, Chasing, Attacking, SeekingHealth }

    [Header("Combat")]
    public float damage = 15f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float detectionRadius = 10f;

    private Rigidbody2D rb;
    private Transform player;
    private Health health;
    private float attackTimer;
    private State currentState = State.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        var p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null || health == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        // Transition logic with debug
        Transform nearestHealth = FindNearestHealthPickup();
        if (health.currentHealth <= health.maxHealth * 0.5f && nearestHealth != null)
        {
            currentState = State.SeekingHealth;
            Debug.Log($"[Warrior] Seeking health pickup at {nearestHealth.position}");
        }
        else if (distToPlayer <= attackRange)
        {
            currentState = State.Attacking;
        }
        else if (distToPlayer <= detectionRadius)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Idle;
        }

        // Behavior with debug
        switch (currentState)
        {
            case State.Idle:
                // stand still
                Debug.Log("[Warrior] Idle");
                break;

            case State.Chasing:
                Debug.Log("[Warrior] Chasing player");
                MoveToward(player.position);
                break;

            case State.Attacking:
                Debug.Log("[Warrior] Attacking player");
                AttackPlayer();
                break;

            case State.SeekingHealth:
                if (nearestHealth != null)
                {
                    Debug.Log($"[Warrior] Moving toward health pickup at {nearestHealth.position}");
                    MoveToward(nearestHealth.position);
                }
                break;
        }
    }

    void MoveToward(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        Vector2 newPos = rb.position + dir * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }

    void AttackPlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > attackRange)
        {
            currentState = State.Chasing;
            return;
        }

        if (attackTimer <= 0f)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage((int)damage);

            attackTimer = attackCooldown;
        }
    }

    Transform FindNearestHealthPickup()
    {
        EnemyHealthPickup[] pickups = FindObjectsOfType<EnemyHealthPickup>();
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var pickup in pickups)
        {
            float dist = Vector2.Distance(transform.position, pickup.transform.position);
            if (dist < minDist)
            {
                closest = pickup.transform;
                minDist = dist;
            }
        }

        if (closest == null)
            Debug.Log("[Warrior] No health pickups found");

        return closest;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}



