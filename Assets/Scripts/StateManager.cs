using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStateMode
{
    Edit,
    Play,
    None,
    Interrupted
}

public class StateManager : MonoBehaviour
{
    [SerializeField] private TargetSpawner targetSpawner;

    [SerializeField] private LevelLoadManager levelManager;
    [SerializeField] private TokenSpawner tokenSpawner;
    [SerializeField] private VidgetConfigurator vidgetConfigurator;
    [SerializeField] private GameStateButtonController stateButtonController;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject losePanelPrefab, winPanelPrefab;

    public void StartGame()
    {
        CurrentLevelStats.Reset();
        GameState.EnterPlayMode();
        
        levelManager.FillBGTiles();

        if(ObstaclesPool.Get().Count() == 0)
            levelManager.RestoreObstaclesBackup();

        tokenSpawner.InitRandomTokens();
    }
    public void StartEditor()
    {
        GameState.EnterEditMode();
        CurrentLevelStats.Reset();
        levelManager.RestoreObstaclesBackup();

        vidgetConfigurator.UpdateMovesVidget();
        vidgetConfigurator.UpdateScoreVidget();
        vidgetConfigurator.UpdateTokensVidgets();
    }

    public void StopEditGame()
    {
        GameState.EnterEditMode();
        TokenPool.Clear();
        ObstaclesPool.Get().Clear();

        levelManager.RestoreObstaclesBackup();
        levelManager.RemoveBGTiles();

        SpawnPosesPool.Clear();

        vidgetConfigurator.UpdateMovesVidget();
        vidgetConfigurator.UpdateScoreVidget();
        vidgetConfigurator.UpdateTokensVidgets();
    }
    public void StopPlayGame()
    {
        GameState.EnterPlayMode();
        TokenPool.Clear();

        levelManager.RestoreObstaclesBackup();
        tokenSpawner.InitRandomTokens();

        vidgetConfigurator.UpdateMovesVidget();
        vidgetConfigurator.UpdateScoreVidget();
        vidgetConfigurator.UpdateTokensVidgets();
    }

    public void RestartEditGame()
    {
        StopEditGame();
        ResetTargets();

        stateButtonController.Reset();
    }
    public void RestartPlayGame()
    {
        StopPlayGame();
        ResetTargets();
    }

    private void ResetTargets()
    {
        foreach (var t in TargetsPool.Targets)
            t.IsReady = false;

        vidgetConfigurator.InitializeVidgets();
    }

    public void Restart()
    {
        CurrentLevelStats.Reset();

        if (SceneManager.GetActiveScene().name == "Editor")
            RestartEditGame();
        else
            RestartPlayGame();
    }

    public void EndGame(bool IsWin)
    {
        GameState.Interrupt();
        GameObject go;

        if (IsWin)
        {
            go = Instantiate(winPanelPrefab);
            EffectsPlayer.Instance().Win();
        }
        else
        {
            go = Instantiate(losePanelPrefab);
            EffectsPlayer.Instance().Fail();
        }

        go.transform.SetParent(canvas.transform, false);
        go.GetComponent<GameOverPanelController>().stateManager = this;
    }

    public void UpdateState()
    {
        var isWin = CheckWin();
        var moves = from t in TargetsPool.Targets where t is MoveTarget select (MoveTarget)t;

        MoveTarget moveTarget = null;

        if(moves.Count() > 0)
            moveTarget = moves.ElementAt(0);

        if (isWin)
        {
            EndGame(true);
            return;
        }

        if (moveTarget && moveTarget.IsReady)
            EndGame(false);
    }

    private bool CheckWin()
    {
        bool isOtherCompleted = true;

        foreach (var tar in TargetsPool.Targets)
            if (!(tar is MoveTarget) && !tar.IsReady)
                isOtherCompleted = false;

        return isOtherCompleted;
    }
}
