using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class TilesEditor : MonoBehaviour
{
    [SerializeField] private Tilemap _borderTilemap, _bgTilemap;
    [SerializeField] private TileBase _border, _bg;
    [SerializeField] private Image _currentTool;
    [SerializeField] private Sprite _borderSprite, _bgSprite;

    [SerializeField] private TokenSpawner _spawner;

    private bool _isTokenSpawning = false;
    private GameObject _activeToken;

    private BorderIntegrity _integrity;

    void Start()
    {
        _currentTool.sprite = _borderSprite;
        _integrity = GetComponent<BorderIntegrity>();
    }

    void Update()
    {
        var cellPos = GetCellPos();

        if (Input.GetMouseButton(0) && IsInGridBorder(cellPos) && GameState.IsEditMode() && !_isTokenSpawning)
            Place(cellPos);
        if(Input.GetMouseButtonDown(0) && IsInGridBorder(cellPos) && GameState.IsEditMode() && _isTokenSpawning)
            Place(cellPos);
        if (Input.GetMouseButton(1) && IsInGridBorder(cellPos) && GameState.IsEditMode())
            UnPlace(cellPos);
    }

    public void InitSpawnPoses()
    {
        var pos = _integrity.GetFillPos();
        SpawnPosesPool.Init(pos);
    }

    public void SelectBorder()
    {
        EffectsPlayer.Instance().Click();

        _currentTool.sprite = _borderSprite;
        _isTokenSpawning = false;
    }

    public void SelectTile(Tilemap map, TileBase @base, Sprite sprite)
    {
        EffectsPlayer.Instance().Click();
        _isTokenSpawning = false;

        _currentTool.sprite = sprite;
    }
    public void SelectToken(GameObject token)
    {
        EffectsPlayer.Instance().Click();
        _isTokenSpawning = true;

        _activeToken = token;

        _currentTool.sprite = token.GetComponent<SpriteRenderer>().sprite;
    }

    private void UnPlace(Vector3Int cellPos)
    {
        if (!_isTokenSpawning)
        {
            _borderTilemap.SetTile(cellPos, null);
            BorderPool.Remove(cellPos.ToV2Int());
        }
        else
        {
            var go = from p in TokenPool.Get()
                     where p.GridPos == cellPos.ToV2Int()
                     select p;

            if (go.Count() == 0)
                return;

            var first = go.ElementAt(0);

            if(first.Type == _activeToken.GetComponent<Token>().Type)
            {
                TokenPool.Remove(first);
                ObstaclesPool.Get().Remove(first);

                ObstaclesBackupPool.Get().Remove(new Tuple<GameObject, Vector3Int>(first.gameObject, cellPos));

                Destroy(first.gameObject);
            }
        }
    }
    private void Place(Vector3Int cellPos)
    {
        if(!_isTokenSpawning && _borderTilemap.GetTile(cellPos) == null)
        {
            _borderTilemap.SetTile(cellPos, _border);
            BorderPool.Add(cellPos.ToV2Int());
        }
        else
        {
            if (_borderTilemap.GetTile(cellPos) != null)
                return;

            if ((from p in TokenPool.Get() where p.GridPos == cellPos.ToV2Int() select p).Count() != 0)
                return;

            ObstaclesBackupPool.Add(new Tuple<GameObject, Vector3Int>(_activeToken, cellPos));

            var go = Instantiate(_activeToken);
            go.transform.position = _borderTilemap.GetCellCenterWorld(cellPos);

            var token = go.GetComponent<Token>();
            token.GridPos = cellPos.ToV2Int();

            TokenPool.Add(token);
            ObstaclesPool.Get().Add(token);
        }
    }
    private Vector3Int GetCellPos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _borderTilemap.WorldToCell(mousePos);
    }

    private void Fill(Vector2Int origin)
    {
        var tempPos = new Vector3Int(origin.x, origin.y - 1, 0);

        if (FillIsEmpty(tempPos))
            Fill(tempPos.ToV2Int());

        tempPos = new Vector3Int(origin.x, origin.y + 1, 0);

        if (FillIsEmpty(tempPos))
            Fill(tempPos.ToV2Int());

        tempPos = new Vector3Int(origin.x - 1, origin.y, 0);

        if (FillIsEmpty(tempPos))
            Fill(tempPos.ToV2Int());

        tempPos = new Vector3Int(origin.x + 1, origin.y, 0);

        if (FillIsEmpty(tempPos))
            Fill(tempPos.ToV2Int());
    }
    private bool FillIsEmpty(Vector3Int pos)
    {
        if (_borderTilemap.GetTile(pos) == null && _bgTilemap.GetTile(pos) == null)
        {
            _bgTilemap.SetTile(pos, _bg);
            return true;
        }
        return false;
    }

    private Vector2Int GetFreeCell(Vector2Int startPos)
    {
        startPos.x += 1;
        startPos.y -= 1;

        if (_borderTilemap.GetTile(new Vector3Int(startPos.x, startPos.y, 0)) != null)
            return GetFreeCell(startPos);
        else return startPos;
    }

    private bool IsInGridBorder(Vector3Int selectedCellPos)
    {
        if (selectedCellPos.x >= FieldBounds.LeftBottom.x && selectedCellPos.x <= FieldBounds.RightUpper.x && selectedCellPos.y >= FieldBounds.LeftBottom.y && selectedCellPos.y <= FieldBounds.RightUpper.y)
            return true;
        else
            return false;
    }
}
