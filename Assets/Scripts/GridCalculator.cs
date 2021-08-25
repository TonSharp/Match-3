using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GridCalculator
{
    public static List<Vector2Int> GetNeigbourPos(Vector2Int origin)
    {
        var poses = new List<Vector2Int>();

        for (int y = 1; y >= -1; y--)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (y == x && x == 0)
                    continue;

                poses.Add(new Vector2Int(origin.x + x, origin.y + y));
            }
        }

        return poses;
    }

    public static Vector2[] GetXNeigboursPos(Vector2Int origin, Tilemap map)
    {
        var poses = new Vector2[]
        {
            map.GetCellCenterWorld(new Vector3Int(origin.x, origin.y + 1, 0)),
            map.GetCellCenterWorld(new Vector3Int(origin.x - 1, origin.y, 0)),
            map.GetCellCenterWorld(new Vector3Int(origin.x + 1, origin.y, 0)),
            map.GetCellCenterWorld(new Vector3Int(origin.x, origin.y - 1, 0)),
        };

        return poses;
    }

    public static List<Token> GetNeigbourTokens(Vector2Int origin, Tilemap map)
    {
        var poses = V2IntToV2(GetNeigbourPos(origin), map);
        var objects = new List<Token>();

        RaycastHit2D hit;

        foreach (var pos in poses)
            if ((hit = Physics2D.Raycast(pos, Vector3.forward)) && hit.collider.gameObject.TryGetComponent(out Token token))
                objects.Add(token);

        return objects;
    }
    public static List<Token> GetXNeigbourTokens(Vector2Int origin, Tilemap map)
    {
        var poses = GetXNeigboursPos(origin, map);
        var objects = new List<Token>();

        RaycastHit2D hit;

        foreach (var pos in poses)
            if ((hit = Physics2D.Raycast(pos, Vector3.forward)) && hit.collider.gameObject.TryGetComponent(out Token token))
                objects.Add(token);

        return objects;
    }

    public static List<Vector2> V2IntToV2(List<Vector2Int> vectors, Tilemap map)
    {
        var list = new List<Vector2>();

        foreach(var vector in vectors)
            list.Add(map.GetCellCenterWorld(vector.ToV3Int()));

        return list;
    }

    public static Vector2[] GetFallingPos(Vector2Int origin, Tilemap map)
    {
        var p1 = map.GetCellCenterWorld(new Vector3Int(origin.x, origin.y + 1, 0));
        var p2 = map.GetCellCenterWorld(new Vector3Int(origin.x - 1, origin.y + 1, 0));
        var p3 = map.GetCellCenterWorld(new Vector3Int(origin.x + 1, origin.y + 1, 0));

        var poses = new Vector2[3] { p1, p2, p3 };

        return poses;
    }
}
