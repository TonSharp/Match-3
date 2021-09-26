using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadController : MonoBehaviour
{
    public TargetSpawner spawner;

    [SerializeField] private VidgetConfigurator vidgetConfigurator;
    [SerializeField] private LevelLoadManager loadManager;
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    public void Load(string path)
    {
        if (path == "")
            return;

        loadManager.ClearBorder();

        foreach (var target in TargetsPool.Targets)
            Destroy((target as MonoBehaviour).gameObject);

        using (var stream = new StreamReader(path))
        {
            LoadBorder(stream);
            LoadObstacles(stream);
            LoadTargets(stream);
        }

        loadManager.LoadBorder();

        vidgetConfigurator.InitializeVidgets();

        GameState.EnterEditMode();
        Destroy(gameObject);
    }

    private void LoadObstacles(StreamReader stream)
    {
        string obstacles = stream.ReadLine();

        if (obstacles == null)
            return;

        string[] splited = obstacles.Split();

        int type, lvl, x, y;

        for(int i = 0; i < splited.Length; i++)
        {
            if (splited[i] == "")
                break;

            type = int.Parse(splited[i]);
            i++;

            lvl = int.Parse(splited[i]);
            i++;

            x = int.Parse(splited[i]);
            i++;

            y = int.Parse(splited[i]);

            var go = obstacleSpawner.GetObstaclePrefabByParams((TokenType)type, lvl);

            ObstaclesBackupPool.Add(new System.Tuple<GameObject, Vector3Int>(go, new Vector3Int(x, y, 0)));
        }
    }

    private void LoadTargets(StreamReader stream)
    {
        string targets = stream.ReadLine();

        if (targets == null)
            return;

        string[] splited = targets.Split();

        int num = 0;
        int target = 0, token = 0, count = 0; 

        for (int i = 0; i < splited.Length; i++)
        {
            if (splited[i] == "")
                break;

            num = int.Parse(splited[i]);

            if(num == 0 || num == 1)
            {
                target = num;
                i++;
                count = int.Parse(splited[i]);

                if(num == 0)
                {
                    MoveTarget mt = (MoveTarget)spawner.CreateMoveTarget();
                    mt.inputField.text = count.ToString();

                    CurrentLevelStats.AvailableMoves = count;
                }
                else
                {
                    ScoreTarget st = (ScoreTarget)spawner.CreateScoreTarget();
                    st.inputField.text = count.ToString();

                    CurrentLevelStats.XPTarget = count;
                }
            }
            else
            {
                target = num;
                i++;
                token = int.Parse(splited[i]);

                if(num == 3)
                {
                    ObstacleTarget ot = (ObstacleTarget)spawner.CreateObstacleTarget();
                    ot.Initialize();
                    ot.tokenTypeDropdown.value = token;
                }
                else
                {
                    i++;
                    count = int.Parse(splited[i]);

                    TokenTarget tt = (TokenTarget)spawner.CreateCustomTokenTarget((TokensTypes)token);
                    tt.Initialize();
                    tt.ChangeTokenTypeValue(token);
                    tt.inputField.text = count.ToString();

                    switch(token)
                    {
                        case 0:
                            CurrentLevelStats.RedTarget = count;
                            break;
                        case 1:
                            CurrentLevelStats.GreenTarget = count;
                            break;
                        case 2:
                            CurrentLevelStats.BlueTarget = count;
                            break;
                        case 3:
                            CurrentLevelStats.PinkTarget = count;
                            break;
                        case 4:
                            CurrentLevelStats.YellowTarget = count;
                            break;

                    }
                }
            }
        }
    }

    void LoadBorder(StreamReader stream)
    {
        string borders = stream.ReadLine();

        int pair = 1;
        int x = 0, y = 0;

        foreach (var num in borders.Split())
        {
            if (num == "")
                break;

            if (pair == 1)
            {
                x = int.Parse(num);
                pair++;
            }

            else
            {
                y = int.Parse(num);
                BorderPool.Add(new Vector2Int(x, y));

                pair = 1;
            }
        }
    }
}
