using System.Collections.Generic;
using UnityEngine;

public static class BombRegistry
{
    private static readonly List<IBombable> items = new List<IBombable>();
    private static readonly List<IBombable> buffer = new List<IBombable>();

    public static void Add(IBombable item)
    {
        items.Add(item);
    }

    public static void Remove(IBombable item)
    {
        items.Remove(item);
    }

    public static void BombAll(float bossPercent)
    {
        buffer.Clear();
        buffer.AddRange(items);
        foreach (IBombable item in buffer)
        {
            if (item is Object target && target == null)
            {
                items.Remove(item);
                continue;
            }
            item.OnBomb(bossPercent);
        }
    }
}
