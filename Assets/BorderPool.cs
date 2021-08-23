using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BorderPool : MonoBehaviour
{
    [SerializeField] private HashSet<Vector2Int> _borderPool = new HashSet<Vector2Int>();

    public void AddToPool(Vector2Int pos) => _borderPool.Add(pos);
    public void RemoveFromPool(Vector2Int pos) => _borderPool.Remove(pos);

    public HashSet<Vector2Int> GetPool() => _borderPool;
}
