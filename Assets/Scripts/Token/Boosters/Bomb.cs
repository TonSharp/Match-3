using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

class Bomb : MonoBehaviour, IBooster
{
    public Func<Vector2Int, Tilemap, List<Token>> Activate()
    {
        return GridCalculator.Get2NeigbourTokens;
    }
}
