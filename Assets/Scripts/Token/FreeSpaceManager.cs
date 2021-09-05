using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TokenSpawner))]
public class FreeSpaceManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    private bool _shoulManage = false;
    public TokenSpawner Spawner { get; private set; }

    private void Start()
    {
        Spawner = GetComponent<TokenSpawner>();
    }

    private void Update()
    {
        if (_shoulManage)
            UpdateFreeSpace();
    }

    public void Manage()
    {
        _shoulManage = true;
    }

    private void UpdateFreeSpace()
    {
        var existedTokensPoses = from t in TokenPool.Get()
                                 select t.GridPos;

        var holesPoses = from h in SpawnPosesPool.Get().Except(existedTokensPoses)
                         orderby h.y descending
                         select h;

        if (holesPoses.Count() == 0)
            _shoulManage = false;

        foreach(var holePos in holesPoses)
        {
            var fills = GetPotentialFills(holePos);

            if (fills.Count == 0 && !IsAnyHigher(holePos))
                Spawner.SpawnRandomToken(holePos);
            else
            {
                var moveable = from f in fills
                               where f.IsMoveable == true
                               select f;

                if (moveable.Count() == 0)
                    continue;

                moveable.ElementAt(0).MoveToPos(holePos, _tilemap);
            }
        }
    }
    private List<Token> GetPotentialFills(Vector2Int origin)
    {
        var fills = new List<Token>();

        foreach (var potentialFill in GridCalculator.GetFallingPos(origin))
        {
            if (IsTokenAtPos(potentialFill, out var fill))
                fills.Add(fill);
        }

        return fills;
    }
    private bool IsAnyHigher(Vector2Int origin)
    {
        int current = origin.y + 1;

        while (current <= FieldBounds.RightUpper.y)
        {
            if (IsTokenAtPos(new Vector2Int(origin.x, current), out _))
                return true;

            current++;
        }

        return false;
    }
    private bool IsTokenAtPos(Vector2Int pos, out Token token)
    {
        token = null;

        var tokens = from t in TokenPool.Get()
                     where t.GridPos == pos
                     select t;

        if (tokens.Count() == 0)
            return false;

        token = tokens.ElementAt(0);
        return true;
    }
}
