using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject firstIce, firstStone, secondIce, secondStone;

    public GameObject GetObstaclePrefabByParams(TokenType type, int lvl)
    {
        switch (type)
        {
            case TokenType.Ice:
                if (lvl == 1)
                    return firstIce;
                else
                    return secondIce;

            case TokenType.Stone:
                if (lvl == 1)
                    return firstStone;
                else
                    return secondStone;
        }

        return null;
    }
    public GameObject SpawnObstacleByParams(TokenType type, int lvl)
    {
        var go = Instantiate(GetObstaclePrefabByParams(type, lvl));
        return go;
    }
}
