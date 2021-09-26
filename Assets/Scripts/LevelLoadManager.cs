using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoadManager : MonoBehaviour
{
    [SerializeField] private Tilemap borderTilemap, bgTilemap;
    [SerializeField] private TileBase border, bg;
    [SerializeField] private BorderIntegrity integrity;

    public void LoadBorder()
    {
        foreach (var border in BorderPool.Get())
            borderTilemap.SetTile(border.ToV3Int(), this.border);
    }

    public void FillBGTiles()
    {
        InitSpawnPoses();

        foreach (var p in SpawnPosesPool.Get())
            bgTilemap.SetTile(p.ToV3Int(), bg);
    }
    public void RemoveBGTiles()
    {
        foreach (var p in SpawnPosesPool.Get())
            bgTilemap.SetTile(p.ToV3Int(), null);
    }

    public void ClearBorder()
    {
        foreach (var border in BorderPool.Get())
            borderTilemap.SetTile(border.ToV3Int(), null);

        BorderPool.Get().Clear();
    }

    public void InitSpawnPoses()
    {
        var pos = integrity.GetFillPos();
        SpawnPosesPool.Init(pos);
    }

    public void RestoreObstaclesBackup()
    {
        foreach (var ob in ObstaclesBackupPool.Get())
        {
            var go = Instantiate(ob.Item1);
            go.transform.position = borderTilemap.GetCellCenterWorld(ob.Item2);

            var token = go.GetComponent<Token>();
            token.GridPos = ob.Item2.ToV2Int();

            TokenPool.Add(token);
            ObstaclesPool.Get().Add(token);
        }
    }
}
