using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Health))]
public class LavaDamage : MonoBehaviour
{
    public Tilemap lavaTilemap;    // Assign in Inspector
    public int damagePerSecond = 10;

    private Health playerHealth;
    private float damageTimer = 0f;

    void Start()
    {
        playerHealth = GetComponent<Health>();
    }

    void Update()
    {
        Vector3Int cellPosition = lavaTilemap.WorldToCell(transform.position);

        if (lavaTilemap.HasTile(cellPosition))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f) // 1 second interval
            {
                playerHealth.TakeDamage(damagePerSecond);
                damageTimer = 0f;
            }
        }
        else
        {
            damageTimer = 0f; // reset if not on lava
        }
    }
}
