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
    [SerializeField] private TileBase _bg, _border;
    [SerializeField] private Image _currentTool;
    [SerializeField] private Sprite _borderSprite, _bgSprite;

    [SerializeField] private TokenSpawner _spawner;

    private Tilemap _activeTilemap;
    private TileBase _activeBase;

    private bool _isTokenSpawning = false;
    private GameObject _activeToken;

    private BorderIntegrity _integrity;

    void Start()
    {
        _activeTilemap = _borderTilemap;
        _activeBase = _border;

        _currentTool.sprite = _borderSprite;

        _integrity = GetComponent<BorderIntegrity>();
    }

    void Update()
    {
        var cellPos = GetCellPos();

        if (Input.GetMouseButton(0) && IsInGridBorder(cellPos) && !GameState.IsPlayMode && GameState.IsEditMode && !_isTokenSpawning)
            Place(cellPos);
        if(Input.GetMouseButtonDown(0) && IsInGridBorder(cellPos) && !GameState.IsPlayMode && GameState.IsEditMode &&  _isTokenSpawning)
            Place(cellPos);
        if (Input.GetMouseButton(1) && IsInGridBorder(cellPos) && !GameState.IsPlayMode && GameState.IsEditMode)
            UnPlace(cellPos);

        if (Input.GetKeyDown(KeyCode.C))
            Debug.Log(_integrity.IsCompleted(out _));

        if (Input.GetKeyDown(KeyCode.F) && GameState.IsEditMode)
        {
            EnterPlayMode();
        }

    }

    private void EnterPlayMode()
    {
        GameState.EnterPlayMode();

        var pos = _integrity.GetFillPos();

        foreach (var p in pos)
            _bgTilemap.SetTile(p.ToV3Int(), _bg);

        _spawner.SpawnRandomTokens(pos);
        SpawnPosesPool.Init(pos);
    }

    public void SelectBackground()
    {
        _activeTilemap = _bgTilemap;
        _activeBase = _bg;

        _currentTool.sprite = _bgSprite;
    }

    public void SelectBorder()
    {
        _activeTilemap = _borderTilemap;
        _activeBase = _border;

        _currentTool.sprite = _borderSprite;
    }

    public void SelectTile(Tilemap map, TileBase @base, Sprite sprite)
    {
        _isTokenSpawning = false;

        _activeTilemap = map;
        _activeBase = @base;

        _currentTool.sprite = sprite;
    }
    public void SelectToken(GameObject token)
    {
        _isTokenSpawning = true;

        _activeToken = token;

        _currentTool.sprite = token.GetComponent<SpriteRenderer>().sprite;
    }

    private void UnPlace(Vector3Int cellPos)
    {
        if (!_isTokenSpawning)
        {
            _activeTilemap.SetTile(cellPos, null);
            BorderPool.Remove(cellPos.ToV2Int());
        }
        else
        {
            var go = from p in TokenPool.Get()
                     where p.GridPos == cellPos.ToV2Int()
                     select p;

            if (go.Count() == 0)
                return;

            if(go.ElementAt(0).Type == _activeToken.GetComponent<Token>().Type)
            {
                Destroy(go.ElementAt(0).gameObject);
                TokenPool.Remove(go.ElementAt(0));
            }
        }
    }
    private void Place(Vector3Int cellPos)
    {
        if(!_isTokenSpawning && _activeTilemap.GetTile(cellPos) == null)
        {
            _activeTilemap.SetTile(cellPos, _activeBase);
            BorderPool.Add(cellPos.ToV2Int());
        }
        else
        {
            if (_borderTilemap.GetTile(cellPos) != null)
                return;

            var go = Instantiate(_activeToken);
            go.transform.position = _activeTilemap.GetCellCenterWorld(cellPos);

            var token = go.GetComponent<Token>();
            token.GridPos = cellPos.ToV2Int();

            TokenPool.Add(go.GetComponent<Token>());
        }
    }
    private Vector3Int GetCellPos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _activeTilemap.WorldToCell(mousePos);
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
