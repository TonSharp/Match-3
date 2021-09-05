using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class Rocket : MonoBehaviour, IBooster
{
    public Func<Vector2Int, Tilemap, List<Token>> Activate()
    {
        return GridCalculator.GetLineTokens;
    }
}
