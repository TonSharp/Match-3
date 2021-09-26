using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpawnPosesPool
{
    private static HashSet<Vector2Int> pool = new HashSet<Vector2Int>();

    public static void Init(IEnumerable<Vector2Int> vectors)
    {
        foreach (var vector in vectors)
            pool.Add(vector);
    }

    public static HashSet<Vector2Int> Get() => pool;

    public static void Add(Vector2Int vector) => pool.Add(vector);
    public static void Remove(Vector2Int vector) => pool.Remove(vector);
    public static void Clear()
    {
        pool.Clear();
    }
}
