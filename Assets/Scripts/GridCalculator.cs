using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GridCalculator
{
    public static List<Vector2Int> GetNeighbourPos(Vector2Int origin)
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
    public static List<Vector2Int> Get2NeighbourPos(Vector2Int origin)
    {
        var poses = new List<Vector2Int>();

        for (int y = 2; y >= -2; y--)
        {
            for (int x = -2; x <= 2; x++)
            {
                if (y == x && x == 0)
                    continue;

                poses.Add(new Vector2Int(origin.x + x, origin.y + y));
            }
        }

        return poses;
    }
    public static Vector2[] GetXNeighboursPos(Vector2Int origin, Tilemap map)
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
    public static List<Vector2Int> GetLinePos(Vector2Int origin)
    {
        var poses = new List<Vector2Int>();

        for(int x = FieldBounds.LeftBottom.x; x <= FieldBounds.RightUpper.x; x++)
        {
            if (x == origin.x)
                continue;

            poses.Add(new Vector2Int(x, origin.y));
        }

        return poses;
    }

    public static List<Token> GetNeighbourTokens(Vector2Int origin, Tilemap map)
    {
        var poses = V2IntToV2(GetNeighbourPos(origin), map);
        var objects = new List<Token>();

        RaycastHit2D hit;

        foreach (var pos in poses)
            if ((hit = Physics2D.Raycast(pos, Vector3.forward)) && hit.collider.gameObject.TryGetComponent(out Token token))
                objects.Add(token);

        return objects;
    }
    public static List<Token> Get2NeigbourTokens(Vector2Int origin, Tilemap map)
    {
        var poses = V2IntToV2(Get2NeighbourPos(origin), map);
        var objects = new List<Token>();

        RaycastHit2D hit;

        foreach (var pos in poses)
            if ((hit = Physics2D.Raycast(pos, Vector3.forward)) && hit.collider.gameObject.TryGetComponent(out Token token))
                objects.Add(token);

        return objects;
    }
    public static List<Token> GetXNeighbourTokens(Vector2Int origin, Tilemap map)
    {
        var poses = GetXNeighboursPos(origin, map);
        var objects = new List<Token>();

        RaycastHit2D hit;

        foreach (var pos in poses)
            if ((hit = Physics2D.Raycast(pos, Vector3.forward)) && hit.collider.gameObject.TryGetComponent(out Token token))
                objects.Add(token);

        return objects;
    }
    public static List<Token> GetLineTokens(Vector2Int origin, Tilemap map)
    {
        var poses = V2IntToV2(GetLinePos(origin), map);
        var objects = new List<Token>();
        RaycastHit2D hit;

        foreach (var pos in poses)
        {
            if ((hit = Physics2D.Raycast(pos, Vector3.forward)) && hit.collider.gameObject.TryGetComponent(out Token token))
                objects.Add(token);
        }

        return objects;
    }

    public static List<Vector2> V2IntToV2(List<Vector2Int> vectors, Tilemap map)
    {
        var list = new List<Vector2>();

        foreach(var vector in vectors)
            list.Add(map.GetCellCenterWorld(vector.ToV3Int()));

        return list;
    }

    public static Vector2Int[] GetFallingPos(Vector2Int origin)
    {
        var p1 = new Vector2Int(origin.x, origin.y + 1);
        var p2 = new Vector2Int(origin.x - 1, origin.y + 1);
        var p3 = new Vector2Int(origin.x + 1, origin.y + 1);

        var poses = new Vector2Int[3] { p1, p2, p3 };

        return poses;
    }
}
