using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolsManager
{
    public static void ClearPools()
    {
        SpawnPosesPool.Get().Clear();
        BorderPool.Get().Clear();
        TokenPool.Clear();
        ObstaclesPool.Get().Clear();
        ObstaclesBackupPool.Get().Clear();
        TargetsPool.Targets.Clear();
    }
}
