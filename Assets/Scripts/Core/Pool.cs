using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Stack<GameObject> free = new Stack<GameObject>();

    public Pool(GameObject prefab, Transform parent, int prewarm)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < prewarm; i++)
        {
            free.Push(Create());
        }
    }

    private GameObject Create()
    {
        GameObject item = Object.Instantiate(prefab, parent);
        item.SetActive(false);
        PooledObject marker = item.AddComponent<PooledObject>();
        marker.Setup(this);
        return item;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject item = free.Count > 0 ? free.Pop() : Create();
        item.transform.SetPositionAndRotation(position, rotation);
        item.SetActive(true);
        foreach (IPoolable poolable in item.GetComponent<PooledObject>().Poolables)
        {
            poolable.OnSpawn();
        }
        return item;
    }

    public void Release(GameObject item)
    {
        if (!item.activeSelf)
        {
            return;
        }
        foreach (IPoolable poolable in item.GetComponent<PooledObject>().Poolables)
        {
            poolable.OnDespawn();
        }
        item.SetActive(false);
        free.Push(item);
    }
}
