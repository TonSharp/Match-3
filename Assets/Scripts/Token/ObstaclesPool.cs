using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObstaclesPool
{
    private static HashSet<Token> pool = new HashSet<Token>(); 

    public static int GetIceCount()
    {
        return (from o in pool where o.Type == TokenType.Ice select o).Count();
    }
    public static int GetStoneCount()
    {
        return (from o in pool where o.Type == TokenType.Stone select o).Count();
    }
    public static HashSet<Token> Get()
    {
        return pool;
    }
}
