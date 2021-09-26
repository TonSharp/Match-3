using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Threading.Tasks;

public class TokenSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap _tokensTilemap;

    [SerializeField] private GameObject[] _tokenPrefabs;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _rocketPrefab;

    [SerializeField] private GameObject[] _stonePrefabs;
    [SerializeField] private GameObject[] _icePrefabs;

    public void InitRandomTokens()
    {
        SpawnRandomTokens(SpawnPosesPool.Get().ToList());
    }

    public void SpawnRandomTokens(List<Vector2Int> pos)
    {
        int randId;

        foreach (var p in pos)
        {
            var tokensInPos = from t in TokenPool.Get()
                              where t.GridPos == p
                              select t;

            if (tokensInPos.Count() > 0)
                continue;

            randId = Random.Range(0, _tokenPrefabs.Length);

            var go = Instantiate(_tokenPrefabs[randId]);
            var token = go.GetComponent<Token>();

            go.transform.position = _tokensTilemap.GetCellCenterWorld(p.ToV3Int());
            token.GridPos = p;

            TokenPool.Add(go.GetComponent<Token>());
        }
    }

    public void SpawnRandomToken(Vector2Int pos)
    {
        int randId = Random.Range(0, _tokenPrefabs.Length);
        SpawnToken(pos, _tokenPrefabs[randId]);
    }

    public void SpawnBomb(Vector2Int pos)
    {
        SpawnToken(pos, _bombPrefab);
    }

    public void SpawnRocket(Vector2Int pos)
    {
        SpawnToken(pos, _rocketPrefab);
    }

    private void SpawnToken(Vector2Int pos, GameObject prefab)
    {
        var go = Instantiate(prefab);
        go.transform.position = _tokensTilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y + 1, 0));

        var token = go.GetComponent<Token>();
        token.GridPos = new Vector2Int(pos.x, pos.y + 1);
        token.MoveToPos(pos, _tokensTilemap);

        TokenPool.Add(token);
    }
}
