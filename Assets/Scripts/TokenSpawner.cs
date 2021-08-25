using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Threading.Tasks;

public class TokenSpawner : MonoBehaviour
{
    public HashSet<Token> TokensPool { get; set; } = new HashSet<Token>();

    [SerializeField] private Tilemap _tokensTilemap;

    [SerializeField] private GameObject[] _tokenPrefabs;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _rocketPrefab;

    [SerializeField] private GameObject[] _stonePrefabs;
    [SerializeField] private GameObject[] _icePrefabs;

    private bool _playMode = false;

    public void SpawnRandomTokens(List<Vector2Int> pos)
    {
        _playMode = true;

        int randId;

        foreach (var p in pos)
        {
            var tokensInPos = from t in TokensPool
                              where t.GridPos == p
                              select t;

            if (tokensInPos.Count() > 0)
                continue;

            randId = Random.Range(0, _tokenPrefabs.Length);

            var go = Instantiate(_tokenPrefabs[randId]);
            var token = go.GetComponent<Token>();

            go.transform.position = _tokensTilemap.GetCellCenterWorld(p.ToV3Int());
            token.GridPos = p;

            TokensPool.Add(go.GetComponent<Token>());
        }
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0) && _playMode)
            TryDestroyToken();
    }
    private void TryDestroyToken()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var gridPos = _tokensTilemap.WorldToCell(mousePos);

        if(IsTokenAtPos(gridPos.ToV2Int(), out var token))
        {
            if (token.LVL > 1)
            {
                token.Downgrade();
                return;
            }

            var go = token.gameObject;

            var tokenPos = token.GridPos;
            var neigbourPos = GridCalculator.GetFallingPos(tokenPos, _tokensTilemap);

            var obstacles = from obj in GridCalculator.GetXNeigbourTokens(tokenPos, _tokensTilemap)
                            where obj.Type == TokenType.Ice || obj.Type == TokenType.Stone
                            select obj;

            foreach (var tPos in neigbourPos)
            {
                if (IsTokenAtPos(tokenPos, out var nToken) && nToken.IsMoveable)
                {
                    FillFallingTokenPos(nToken, tokenPos);

                    break;
                }
            }

            foreach (var obstacle in obstacles)
            {
                if (obstacle.LVL == 2)
                    obstacle.Downgrade();
                else
                    DestroyObstacle(obstacle);
            }

            TokensPool.Remove(token);
            Destroy(go);
        }
    }

    private void DestroyObstacle(Token obstacle)
    {
        var obstaclePos = obstacle.GridPos;
        var obstacleNeigbours = GridCalculator.GetFallingPos(obstaclePos, _tokensTilemap);

        foreach (var potentialFill in obstacleNeigbours)
        {
            var potentialPos = _tokensTilemap.WorldToCell(potentialFill);
            if (IsTokenAtPos(potentialPos.ToV2Int(), out var nToken))
            {
                FillFallingTokenPos(nToken, obstaclePos);

                break;
            }
        }

        TokensPool.Remove(obstacle);
        Destroy(obstacle.gameObject);
    }

    private void FillFallingTokenPos(Token falling, Vector2Int fallingDest)
    {
        var neigboursPoses = GridCalculator.GetFallingPos(falling.GridPos, _tokensTilemap);

        foreach(var neigbourPos in neigboursPoses)
        {
            var pos = _tokensTilemap.WorldToCell(neigbourPos);
            if (IsTokenAtPos(pos.ToV2Int(), out var nToken) && nToken.IsMoveable)
            {
                FillFallingTokenPos(nToken, falling.GridPos);

                break;
            }
        }

        falling.MoveToPos(fallingDest, _tokensTilemap);
    }

    private bool IsTokenAtPos(Vector2Int pos, out Token token)
    {
        token = null;

        var tokens = from t in TokensPool
                     where t.GridPos == pos
                     select t;

        if (tokens.Count() == 0)
            return false;

        token = tokens.ElementAt(0);
        return true;
    }
}
