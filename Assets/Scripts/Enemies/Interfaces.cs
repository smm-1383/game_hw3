public interface IDamageable
{
    void TakeDamage(int amount);
}

public interface IBombable
{
    void OnBomb(float bossPercent);
}

public interface IHazard
{
    void OnHitPlayer();
}
