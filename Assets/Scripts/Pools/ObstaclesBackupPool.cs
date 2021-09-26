using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ObstaclesBackupPool
{
    private static HashSet<Tuple<GameObject, Vector3Int>> pool = new HashSet<Tuple<GameObject, Vector3Int>>();

    public static HashSet<Tuple<GameObject, Vector3Int>> Get() => pool;
    public static void Add(Tuple<GameObject, Vector3Int> backup) => pool.Add(backup);
}
