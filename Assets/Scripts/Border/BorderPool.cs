using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class BorderPool
{
    private static HashSet<Vector2Int> _borderPool = new HashSet<Vector2Int>();

    public static void Add(Vector2Int pos) => _borderPool.Add(pos);
    public static void Remove(Vector2Int pos) => _borderPool.Remove(pos);

    public static HashSet<Vector2Int> Get() => _borderPool;
}
