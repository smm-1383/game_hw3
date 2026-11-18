using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int damage;
    private float speed;

    public void Launch(int bulletDamage, float bulletSpeed, Color color)
    {
        damage = bulletDamage;
        speed = bulletSpeed;
        spriteRenderer.color = color;
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        Vector3 position = transform.position;
        bool outside = position.y > ScreenArea.Top + 1f || position.x < ScreenArea.Left - 1f || position.x > ScreenArea.Right + 1f;
        if (outside)
        {
            PoolManager.Instance.Release(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage);
            PoolManager.Instance.Release(gameObject);
        }
    }
}
