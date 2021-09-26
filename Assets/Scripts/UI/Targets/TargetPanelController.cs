using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPanelController : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;
    [SerializeField] private SaveDialogController saveController;

    [SerializeField] private TargetSpawner targetSpawner;
    [SerializeField] private VidgetConfigurator vidgetSpawner;

    public void MovesTarget()
    {
        EffectsPlayer.Instance().Click();
        targetSpawner.CreateMoveTarget();
    }
    public void ScoreTarget()
    {
        EffectsPlayer.Instance().Click();
        targetSpawner.CreateScoreTarget();
    }
    public void TokenTarget()
    {
        EffectsPlayer.Instance().Click();
        targetSpawner.CreateTokenTarget();
    }

    public void OnDeleteButtonClicked()
    {
        EffectsPlayer.Instance().Click();
        targetSpawner.DeleteLastTarget();
    }

    public void OnCloseButtonClicked()
    {
        EffectsPlayer.Instance().Click();
        if (GameState.CurrentGameState == GameStateMode.Interrupted)
            return;

        GameState.EnterEditMode();
        targetPanel.SetActive(false);

        vidgetSpawner.InitializeVidgets();
        CurrentLevelStats.Initialize();
    }

    public void OnOpenButtonClicked()
    {
        EffectsPlayer.Instance().Click();
        GameState.Stop();

        targetPanel.SetActive(true);
    }

    public void OnSaveButtonClicked()
    {
        EffectsPlayer.Instance().Click();
        saveController.ShowSaveDialog();
    }
}
