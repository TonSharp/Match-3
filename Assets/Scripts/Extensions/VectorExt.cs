using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2IntExt
{
    public static Vector3Int ToV3Int(this Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }
}
public static class Vector3IntExt
{
    public static Vector2Int ToV2Int(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }
}
public static class Vector3Ext
{
    public static Vector2 ToV2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
}

