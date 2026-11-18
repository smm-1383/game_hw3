using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public Pool Owner { get; private set; }
    public IPoolable[] Poolables { get; private set; }

    public void Setup(Pool owner)
    {
        Owner = owner;
        Poolables = GetComponents<IPoolable>();
    }
}
