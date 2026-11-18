using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolEntry
{
    public GameObject prefab;
    public int count;
}

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("PoolManager").AddComponent<PoolManager>();
            }
            return instance;
        }
    }

    [SerializeField] private PoolEntry[] prewarm;

    private readonly Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();

    private void Awake()
    {
        instance = this;
        if (prewarm == null)
        {
            return;
        }
        foreach (PoolEntry entry in prewarm)
        {
            pools[entry.prefab] = new Pool(entry.prefab, transform, entry.count);
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(prefab, out Pool pool))
        {
            pool = new Pool(prefab, transform, 0);
            pools[prefab] = pool;
        }
        return pool.Get(position, rotation);
    }

    public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        return Get(prefab.gameObject, position, rotation).GetComponent<T>();
    }

    public void Release(GameObject item)
    {
        item.GetComponent<PooledObject>().Owner.Release(item);
    }
}
