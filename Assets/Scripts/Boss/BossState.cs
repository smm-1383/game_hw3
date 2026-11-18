public abstract class BossState
{
    protected readonly Boss boss;

    protected BossState(Boss boss)
    {
        this.boss = boss;
    }

    public virtual void Enter()
    {
    }

    public abstract void Tick();

    public virtual void Exit()
    {
    }
}
