using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TokenPool
{
    private static HashSet<Token> _pool = new HashSet<Token>();

    public static HashSet<Token> Get() => _pool;

    public static void Add(Token token) => _pool.Add(token);
    public static void Remove(Token token) => _pool.Remove(token);

    public static void Clear()
    {
        try
        {
            foreach (var t in _pool)
                Object.Destroy(t.gameObject);
        }
        catch { }

        _pool.Clear();
    }

    public static void UpdateObstaclesPool()
    {
        if ((from p in _pool where p.Type == TokenType.Ice || p.Type == TokenType.Stone select p).Count() == 0)
            ObstaclesPool.Get().Clear();
    }
}
