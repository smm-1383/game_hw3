using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable, IBombable, IPoolable, IHazard
{
    [SerializeField] protected EnemyDataSO data;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashTime = 0.06f;

    protected int hp;
    private bool dead;
    private float flashTimer;
    private Material normalMaterial;

    public event Action<Enemy> Removed;

    protected virtual void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        normalMaterial = spriteRenderer.sharedMaterial;
    }

    protected virtual void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f)
            {
                spriteRenderer.sharedMaterial = normalMaterial;
            }
        }
    }

    public virtual void Setup(EnemyDataSO enemyData, int startHp)
    {
        data = enemyData;
        hp = startHp;
        if (data.sprite != null)
        {
            spriteRenderer.sprite = data.sprite;
        }
        spriteRenderer.color = data.tint;
    }

    public virtual void OnSpawn()
    {
        dead = false;
        flashTimer = 0f;
        spriteRenderer.sharedMaterial = normalMaterial;
        BombRegistry.Add(this);
    }

    public virtual void OnDespawn()
    {
        BombRegistry.Remove(this);
        Removed = null;
    }

    public virtual void TakeDamage(int amount)
    {
        if (dead)
        {
            return;
        }
        hp -= amount;
        Flash();
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Flash()
    {
        if (flashMaterial == null)
        {
            return;
        }
        spriteRenderer.sharedMaterial = flashMaterial;
        flashTimer = flashTime;
    }

    public virtual void OnBomb(float bossPercent)
    {
        if (!dead)
        {
            Die();
        }
    }

    public virtual void OnHitPlayer()
    {
        Die();
    }

    protected virtual void Die()
    {
        dead = true;
        GameManager.Instance.AddScore(data.score);
        Drop();
        if (data.deathEffect != null)
        {
            PoolManager.Instance.Get(data.deathEffect, transform.position, Quaternion.identity);
        }
        Remove();
    }

    private void Drop()
    {
        if (data.foodPrefab != null && UnityEngine.Random.value < data.foodDropChance)
        {
            PoolManager.Instance.Get(data.foodPrefab, transform.position, Quaternion.identity);
        }
        if (data.giftPrefab != null && UnityEngine.Random.value < data.giftDropChance)
        {
            PoolManager.Instance.Get(data.giftPrefab, transform.position, Quaternion.identity);
        }
    }

    protected void Remove()
    {
        Action<Enemy> callback = Removed;
        PoolManager.Instance.Release(gameObject);
        callback?.Invoke(this);
    }
}
