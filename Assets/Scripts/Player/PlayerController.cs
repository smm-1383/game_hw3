using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private Vector2 margin = new Vector2(0.6f, 0.6f);
    [SerializeField] private float invincibleTime = 1.5f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private SpriteRenderer shipSprite;
    [SerializeField] private WeaponSystem weapon;
    [SerializeField] private MissileLauncher missileLauncher;
    [SerializeField] private GameObject deathEffect;

    private Rigidbody2D body;
    private Vector2 moveInput;
    private bool invincible;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        weapon.Firing = value.isPressed;
    }

    public void OnMissile(InputValue value)
    {
        if (value.isPressed)
        {
            missileLauncher.Launch();
        }
    }

    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void FixedUpdate()
    {
        Vector2 target = body.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        target.x = Mathf.Clamp(target.x, ScreenArea.Left + margin.x, ScreenArea.Right - margin.x);
        target.y = Mathf.Clamp(target.y, ScreenArea.Bottom + margin.y, ScreenArea.Top - margin.y);
        body.MovePosition(target);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invincible || !other.TryGetComponent(out IHazard hazard))
        {
            return;
        }
        hazard.OnHitPlayer();
        TakeHit();
    }

    private void TakeHit()
    {
        GameManager game = GameManager.Instance;
        game.LoseLife();
        game.AddPower(-1);
        GameEvents.PlayerDamaged?.Invoke();
        if (game.Player.lives <= 0)
        {
            Die();
            return;
        }
        StartCoroutine(Invincibility());
    }

    private void Die()
    {
        if (deathEffect != null)
        {
            PoolManager.Instance.Get(deathEffect, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    private IEnumerator Invincibility()
    {
        invincible = true;
        float timer = 0f;
        while (timer < invincibleTime)
        {
            shipSprite.enabled = !shipSprite.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }
        shipSprite.enabled = true;
        invincible = false;
    }
}
