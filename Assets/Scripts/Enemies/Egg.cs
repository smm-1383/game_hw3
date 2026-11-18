using UnityEngine;

public class Egg : MonoBehaviour, IDamageable, IBombable, IPoolable, IHazard
{
    [SerializeField] private float fallSpeed = 4f;
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] variants;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.25f);

    private Vector2 direction;
    private float speed;
    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    public void OnSpawn()
    {
        spriteRenderer.sprite = variants[Random.Range(0, variants.Length)];
        transform.localScale = baseScale * Random.Range(scaleRange.x, scaleRange.y);
        direction = Vector2.down;
        speed = fallSpeed;
        BombRegistry.Add(this);
    }

    public void OnDespawn()
    {
        BombRegistry.Remove(this);
    }

    public void Launch(Vector2 launchDirection, float launchSpeed)
    {
        direction = launchDirection.normalized;
        speed = launchSpeed;
    }

    public void Aim(Vector3 target)
    {
        Vector2 aimed = (target - transform.position).normalized;
        aimed.y = Mathf.Min(aimed.y, -0.7f);
        direction = aimed.normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        Vector3 position = transform.position;
        bool outside = position.y < ScreenArea.Bottom - 1f || position.x < ScreenArea.Left - 1f || position.x > ScreenArea.Right + 1f;
        if (outside)
        {
            PoolManager.Instance.Release(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        Break();
    }

    public void OnBomb(float bossPercent)
    {
        Break();
    }

    public void OnHitPlayer()
    {
        Break();
    }

    private void Break()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (breakEffect != null)
        {
            PoolManager.Instance.Get(breakEffect, transform.position, Quaternion.identity);
        }
        PoolManager.Instance.Release(gameObject);
    }
}
