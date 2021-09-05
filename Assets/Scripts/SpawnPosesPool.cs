using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpawnPosesPool
{
    private static HashSet<Vector2Int> _posesPool = new HashSet<Vector2Int>();

    public static void Init(IEnumerable<Vector2Int> vectors)
    {
        foreach (var vector in vectors)
            _posesPool.Add(vector);
    }

    public static HashSet<Vector2Int> Get() => _posesPool;

    public static void Add(Vector2Int vector) => _posesPool.Add(vector);
    public static void Remove(Vector2Int vector) => _posesPool.Remove(vector);
}
