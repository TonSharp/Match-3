using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class BorderIntegrity : MonoBehaviour
{
    [SerializeField] private Tilemap _borderTilemap;

    public List<Vector2Int> GetFillPos()
    {
        var startPos = GetStartFillPoint();
        var list = new List<Vector2Int>();

        if(startPos.HasValue)
            GetFill(startPos.Value, list);

        return list;
    }

    //TODO LineFill
    private void GetFill(Vector2Int pos, List<Vector2Int> filled)
    {
        filled.Add(pos);

        var tempPos = new Vector2Int(pos.x, pos.y + 1);

        if (IsEmpty(_borderTilemap, tempPos) && !filled.Contains(tempPos))
            GetFill(tempPos, filled);

        tempPos.y -= 2;

        if (IsEmpty(_borderTilemap, tempPos) && !filled.Contains(tempPos))
            GetFill(tempPos, filled);

        tempPos.y += 1;
        tempPos.x -= 1;

        if (IsEmpty(_borderTilemap, tempPos) && !filled.Contains(tempPos))
            GetFill(tempPos, filled);

        tempPos.x += 2;

        if (IsEmpty(_borderTilemap, tempPos) && !filled.Contains(tempPos))
            GetFill(tempPos, filled);
    }
    private bool IsEmpty(Tilemap map, Vector2Int pos)
    {
        if (map.GetTile(pos.ToV3Int()) == null)
            return true;
        return false;
    }

    private Vector2Int? GetStartFillPoint()
    {
        if (IsCompleted(out var border))
        {
            foreach (var tile in border)
            {
                var currentStartPoint = GetFirstYFreeSpace(tile);
                if (GetIntersectionsCount(currentStartPoint) % 2 != 0)
                {
                    return currentStartPoint;
                }
            }
        }

        return null;
    }
    private Vector2Int GetFirstYFreeSpace(Vector2Int origin)
    {
        while(_borderTilemap.GetTile(origin.ToV3Int()) != null)
        {
            origin.y--;
        }

        return origin;
    }

    private int GetIntersectionsCount(Vector2Int pos)
    {
        int currentY = pos.y - 1;
        int intersections = 0;

        while(currentY >= FieldBounds.LeftBottom.y)
        {
            if (_borderTilemap.GetTile(new Vector3Int(pos.x, currentY, 0)) != null)
                intersections++;

            currentY--;
        }

        return intersections;
    }

    public bool IsCompleted(out List<Vector2Int> border)
    {
        int startPoint = 0;
        border = new List<Vector2Int>();
        bool isCompleted = false;

        var pool = BorderPool.Get();

        while (border.Count <= 1 && startPoint < pool.Count)
        {
            border.Add(pool.ElementAt(startPoint));

            CalculateBorder(border, 0, out isCompleted);
            if (!isCompleted)
            {
                border.RemoveAt(0);
                startPoint++;
            }
            else
                return true;
        }

        return isCompleted;
    }

    private List<Vector2Int> CalculateBorder(List<Vector2Int> border, int startPoint, out bool isCompleted)
    {
        isCompleted = false;
        var pool = BorderPool.Get();

        var head = border[startPoint];
        List<Vector2Int> intersect = GetNeigbourPos(head);
        intersect = intersect.Intersect(pool).ToList();

        if (border.Count > 3 && intersect.Contains(border[0]))
        {
            isCompleted = true;
            return border;
        }       

        intersect = intersect.Except(border).ToList();

        if (intersect.Count() == 0)
            return border;  
        else
        {
            border.Add(intersect.ElementAt(0));
            return CalculateBorder(border, startPoint + 1, out isCompleted);
        }
    }

    public List<Vector2Int> GetNeigbourPos(Vector2Int origin)
    {
        var poses = new List<Vector2Int>();

        for(int y = 1; y >= -1; y--)
        {
            for(int x = -1; x <= 1; x++)
            {
                if (y == x && x == 0)
                    continue;

                poses.Add(new Vector2Int(origin.x + x, origin.y + y));
            }
        }

        return poses;
    }
}
