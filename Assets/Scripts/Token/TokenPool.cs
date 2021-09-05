using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TokenPool
{
    private static readonly HashSet<Token> _pool = new HashSet<Token>();

    public static HashSet<Token> Get() => _pool;

    public static void Add(Token token) => _pool.Add(token);
    public static void Remove(Token token) => _pool.Remove(token);
}
