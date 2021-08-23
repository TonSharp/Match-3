using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class BorderIntegrity : MonoBehaviour
{
    [SerializeField] private Tilemap _borderTilemap;

    private const int _yMax = 5;
    private const int _yMin = -5;
    private const int _xMax = 3;
    private const int _xMin = -4;

    private BorderPool _pool;

    private void Start()
    {
        _pool = GetComponent<BorderPool>();
    }

    public bool IsCompleted()
    {
        int startPoint = 0;
        var list = new List<Vector2Int>();
        bool isCompleted = false;

        var pool = _pool.GetPool();

        while (list.Count <= 1 && startPoint < pool.Count)
        {
            list.Add(pool.ElementAt(startPoint));

            CalculateBorder(list, 0, out isCompleted);
            if (!isCompleted)
            {
                list.RemoveAt(0);
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
        var pool = _pool.GetPool();

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

    private List<Vector2Int> GetNeigbourPos(Vector2Int origin)
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

    public Vector2Int FindTopLeftCorner()
    {
        for (int y = _yMax; y >= _yMin; y--)
        {
            for (int x = _xMin; x <= _xMax; x++)
            {
                if (_borderTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    return new Vector2Int(x, y);
            }
        }

        throw new System.Exception();
    }
}
