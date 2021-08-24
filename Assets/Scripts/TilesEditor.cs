using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public static class Vector2IntExt
{
    public static Vector3Int ToV3Int(this Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }
}
public static class Vector3IntExt
{
    public static Vector2Int ToV2Int(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }
}

public class TilesEditor : MonoBehaviour
{
    [SerializeField] private Tilemap _borderTilemap, _bgTilemap;
    [SerializeField] private TileBase _bg, _border;
    [SerializeField] private Image _currentTool;

    [SerializeField] private Sprite _borderSprite, _bgSprite;

    private Tilemap _activeTilemap;
    private TileBase _activeBase;

    private BorderIntegrity _integrity;
    private BorderPool _borderPool;
    private TokenSpawner _spawner;

    private const int _yMax = 5;
    private const int _yMin = -5;
    private const int _xMax = 3;
    private const int _xMin = -4;

    void Start()
    {
        _activeTilemap = _borderTilemap;
        _activeBase = _border;

        _currentTool.sprite = _borderSprite;

        _integrity = GetComponent<BorderIntegrity>();
        _borderPool = GetComponent<BorderPool>();
        _spawner = GetComponent<TokenSpawner>();
    }

    void Update()
    {
        var cellPos = GetCellPos();

        if (Input.GetMouseButton(0) && IsInGridBorder(cellPos))
            Place(cellPos);
        if (Input.GetMouseButton(1) && IsInGridBorder(cellPos))
            UnPlace(cellPos);

        if (Input.GetKeyDown(KeyCode.C))
            Debug.Log(_integrity.IsCompleted(out _));

        if (Input.GetKeyDown(KeyCode.F))
        {
            var pos = _integrity.GetFillPos();

            foreach (var p in pos)
                _bgTilemap.SetTile(p.ToV3Int(), _bg);

            _spawner.SpawnRandomTokens(pos);
        }

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

    private void UnPlace(Vector3Int cellPos)
    {
        _activeTilemap.SetTile(cellPos, null);
        _borderPool.RemoveFromPool(cellPos.ToV2Int());
        //if (_activeBase == _border)
        //    if (_integrity.Check())
        //        Debug.Log("Success");
    }
    private void Place(Vector3Int cellPos)
    {
        _activeTilemap.SetTile(cellPos, _activeBase);
        _borderPool.AddToPool(cellPos.ToV2Int());

        //if(_activeBase == _border)
        //    if (_integrity.Check())
        //        Debug.Log("Success");
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
        if (selectedCellPos.x >= _xMin && selectedCellPos.x <= _xMax && selectedCellPos.y >= _yMin && selectedCellPos.y <= _yMax)
            return true;
        else
            return false;
    }
}
