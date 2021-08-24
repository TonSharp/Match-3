using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TokenSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap _tokensTilemap;

    [SerializeField] private GameObject[] _tokenPrefabs;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private GameObject _rocketPrefab;

    [SerializeField] private GameObject[] _stonePrefabs;
    [SerializeField] private GameObject[] _icePrefabs;

    private HashSet<Token> _tokensPool = new HashSet<Token>();

    public void SpawnRandomTokens(List<Vector2Int> pos)
    {
        int randId;

        foreach (var p in pos)
        {
            randId = Random.Range(0, _tokenPrefabs.Length);

            var go = Instantiate(_tokenPrefabs[randId]);
            var token = go.GetComponent<Token>();

            go.transform.position = _tokensTilemap.GetCellCenterWorld(p.ToV3Int());
            token.GridPos = p;

            _tokensPool.Add(go.GetComponent<Token>());
        }
    }
}
