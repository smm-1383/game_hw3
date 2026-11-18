using UnityEngine;

public class BossStateMachine
{
    public BossState Current { get; private set; }

    public void ChangeState(BossState next)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        string from = Current == null ? "none" : Current.GetType().Name;
        Debug.Log("Boss state: " + from + " -> " + next.GetType().Name);
#endif
        if (Current != null)
        {
            Current.Exit();
        }
        Current = next;
        Current.Enter();
    }

    public void Tick()
    {
        if (Current != null)
        {
            Current.Tick();
        }
    }

    public void Clear()
    {
        Current = null;
    }
}
