using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Weapon weapon;

    [Header("Tilemaps")]
    public Tilemap wallTilemap; // Drag your wall tilemap here

    private Rigidbody2D rb;
    private Health health;

    private Vector2 moveDirection;
    private Vector2 mousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        if (wallTilemap == null)
        {
            Debug.LogError("No wall tilemap assigned to PlayerController2D!");
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            weapon.Fire();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // Calculate potential next position
        Vector2 targetPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Convert to tile position
        Vector3Int tilePos = wallTilemap.WorldToCell(targetPos);

        // Only move if there is NO wall tile at the target tile
        if (wallTilemap.GetTile(tilePos) == null)
        {
            rb.MovePosition(targetPos);
        }

        // Rotate towards mouse
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
