using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private CircleCollider2D body;
    [SerializeField] private float radiusFactor = 0.55f;

    private float speed;
    private float drift;
    private float spin;

    public void Init(EnemyDataSO enemyData, int startHp, WaveDataSO wave, float scale, float fallSpeed)
    {
        Setup(enemyData, startHp);
        Sprite sprite = wave.asteroidSprites[Random.Range(0, wave.asteroidSprites.Length)];
        spriteRenderer.sprite = sprite;
        transform.localScale = new Vector3(scale, scale, 1f);
        body.radius = Mathf.Min(sprite.bounds.extents.x, sprite.bounds.extents.y) * radiusFactor;
        speed = fallSpeed;
        drift = Random.Range(-wave.asteroidDriftMax, wave.asteroidDriftMax);
        float turn = Random.Range(wave.asteroidSpinMin, wave.asteroidSpinMax);
        spin = Random.value < 0.5f ? -turn : turn;
    }

    protected override void Update()
    {
        base.Update();
        transform.position += new Vector3(drift, -speed, 0f) * Time.deltaTime;
        transform.Rotate(0f, 0f, spin * Time.deltaTime);
        if (transform.position.y < ScreenArea.Bottom - 2f)
        {
            Remove();
        }
    }
}
