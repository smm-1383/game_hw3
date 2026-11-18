using UnityEngine;

public abstract class Boss : Enemy
{
    [SerializeField] private int maxHp = 100;
    [SerializeField] private float enrageThreshold = 0.3f;

    protected readonly BossStateMachine machine = new BossStateMachine();
    private int currentMaxHp;

    public float HealthPercent
    {
        get { return Mathf.Clamp01((float)hp / currentMaxHp); }
    }

    public bool IsEnraged
    {
        get { return HealthPercent < enrageThreshold; }
    }

    protected abstract void BuildStates();

    public void Begin(float hpMul)
    {
        currentMaxHp = Mathf.Max(1, Mathf.RoundToInt(maxHp * hpMul));
        Setup(data, currentMaxHp);
        machine.Clear();
        BuildStates();
        GameEvents.BossSpawned?.Invoke(this);
        GameEvents.BossHealthChanged?.Invoke(1f);
    }

    protected override void Update()
    {
        base.Update();
        machine.Tick();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        GameEvents.BossHealthChanged?.Invoke(HealthPercent);
    }

    public override void OnBomb(float bossPercent)
    {
        TakeDamage(Mathf.CeilToInt(currentMaxHp * bossPercent));
    }

    public override void OnHitPlayer()
    {
    }
}
