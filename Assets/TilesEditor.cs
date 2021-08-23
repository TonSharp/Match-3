using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilesEditor : MonoBehaviour
{
    [SerializeField] private Tilemap _borderTilemap, _bgTilemap;
    [SerializeField] private TileBase _bg, _border;
    [SerializeField] private Image _currentTool;

    [SerializeField] private Sprite _borderSprite, _bgSprite;

    private Tilemap _activeTilemap;
    private TileBase _activeBase;

    void Start()
    {
        _activeTilemap = _borderTilemap;
        _activeBase = _border;

        _currentTool.sprite = _borderSprite;
    }

    void Update()
    {
        Debug.Log(GetCellPos());
        if (Input.GetMouseButton(0) && GetCellPos().y >= -5)
            Place();
        if (Input.GetMouseButton(1))
            UnPlace();
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

    private void UnPlace()
    {
        _activeTilemap.SetTile(GetCellPos(), null);
    }
    private void Place()
    {
        _activeTilemap.SetTile(GetCellPos(), _activeBase);
    }
    private Vector3Int GetCellPos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _activeTilemap.WorldToCell(mousePos);
    }
}
