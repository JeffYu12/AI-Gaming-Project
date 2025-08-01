using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25;

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

