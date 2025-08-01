using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RubyController : MonoBehaviour
{
    // ========= MOVEMENT =================
    public float speed = 4;
    public InputAction moveAction;

    // ======== HEALTH ==========
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public Transform respawnPosition;
    public ParticleSystem hitParticle;

    // ======== PROJECTILE ==========
    public GameObject projectilePrefab;
    public InputAction launchAction;

    // ======== HEALTH ==========
    public int health
    {
        get { return currentHealth; }
    }


    // =========== MOVEMENT ==============
    Rigidbody2D rigidbody2d;
    Vector2 currentInput;

    // ======== HEALTH ==========
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;


    void Start()
    {
        // =========== MOVEMENT ==============
        rigidbody2d = GetComponent<Rigidbody2D>();
        moveAction.Enable();

        // ======== HEALTH ==========
        invincibleTimer = -1.0f;
        currentHealth = maxHealth;


        // ==== ACTIONS ====
        launchAction.Enable();

        launchAction.performed += LaunchProjectile;
    }

    void Update()
    {
        // ================= HEALTH ====================
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        // ============== MOVEMENT ======================
        Vector2 move = moveAction.ReadValue<Vector2>();

        currentInput = move;


    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        position = position + currentInput * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    // ===================== HEALTH ==================
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;



            Instantiate(hitParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
            Respawn();

        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        //UIHealthBar.Instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition.position;
    }

    // =============== PROJECTICLE ========================
    void LaunchProjectile(InputAction.CallbackContext context)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();

    }
}

