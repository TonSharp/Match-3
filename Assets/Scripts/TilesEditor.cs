using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class TilesEditor : MonoBehaviour
{
    [SerializeField] private Tilemap borderTilemap, bgTilemap;
    [SerializeField] private TileBase border, bg;
    [SerializeField] private Image currentTool;
    [SerializeField] private Sprite borderSprite, bgSprite;
    [SerializeField] private VidgetConfigurator vidgetConfigurator;

    [SerializeField] private TokenSpawner spawner;

    private bool isTokenSpawning = false;
    private GameObject activeToken;

    private BorderIntegrity integrity;

    void Start()
    {
        currentTool.sprite = borderSprite;
        integrity = GetComponent<BorderIntegrity>();

        vidgetConfigurator.InitializeVidgets();
    }

    void Update()
    {
        var cellPos = GetCellPos();

        if (Input.GetMouseButton(0) && IsInGridBorder(cellPos) && GameState.IsEditMode() && !isTokenSpawning)
            Place(cellPos);
        if(Input.GetMouseButtonDown(0) && IsInGridBorder(cellPos) && GameState.IsEditMode() && isTokenSpawning)
            Place(cellPos);
        if (Input.GetMouseButton(1) && IsInGridBorder(cellPos) && GameState.IsEditMode())
            UnPlace(cellPos);
    }

    public void InitSpawnPoses()
    {
        var pos = integrity.GetFillPos();
        SpawnPosesPool.Init(pos);
    }

    public void SelectBorder()
    {
        EffectsPlayer.Instance().Click();

        currentTool.sprite = borderSprite;
        isTokenSpawning = false;
    }

    public void SelectTile(Tilemap map, TileBase @base, Sprite sprite)
    {
        EffectsPlayer.Instance().Click();
        isTokenSpawning = false;

        currentTool.sprite = sprite;
    }
    public void SelectToken(GameObject token)
    {
        EffectsPlayer.Instance().Click();
        isTokenSpawning = true;

        activeToken = token;

        currentTool.sprite = token.GetComponent<SpriteRenderer>().sprite;
    }

    private void UnPlace(Vector3Int cellPos)
    {
        if (!isTokenSpawning)
        {
            borderTilemap.SetTile(cellPos, null);
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

            if(first.Type == activeToken.GetComponent<Token>().Type)
            {
                TokenPool.Remove(first);
                ObstaclesPool.Get().Remove(first);

                ObstaclesBackupPool.Get().Remove(new Tuple<GameObject, Vector3Int>(first.gameObject, cellPos));

                Destroy(first.gameObject);
            }

            vidgetConfigurator.UpdateObstaclesVidgets();
        }
    }
    private void Place(Vector3Int cellPos)
    {
        if(!isTokenSpawning && borderTilemap.GetTile(cellPos) == null)
        {
            borderTilemap.SetTile(cellPos, border);
            BorderPool.Add(cellPos.ToV2Int());
        }
        else
        {
            if (borderTilemap.GetTile(cellPos) != null)
                return;

            if ((from p in TokenPool.Get() where p.GridPos == cellPos.ToV2Int() select p).Count() != 0)
                return;

            if(!ObstaclesBackupPool.Get().Contains(new Tuple<GameObject, Vector3Int>(activeToken, cellPos)))
                ObstaclesBackupPool.Add(new Tuple<GameObject, Vector3Int>(activeToken, cellPos));

            var go = Instantiate(activeToken);
            go.transform.position = borderTilemap.GetCellCenterWorld(cellPos);

            var token = go.GetComponent<Token>();
            token.GridPos = cellPos.ToV2Int();

            TokenPool.Add(token);
            ObstaclesPool.Get().Add(token);

            vidgetConfigurator.UpdateObstaclesVidgets();
        }
    }
    private Vector3Int GetCellPos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return borderTilemap.WorldToCell(mousePos);
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
        if (borderTilemap.GetTile(pos) == null && bgTilemap.GetTile(pos) == null)
        {
            bgTilemap.SetTile(pos, bg);
            return true;
        }
        return false;
    }

    private Vector2Int GetFreeCell(Vector2Int startPos)
    {
        startPos.x += 1;
        startPos.y -= 1;

        if (borderTilemap.GetTile(new Vector3Int(startPos.x, startPos.y, 0)) != null)
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
