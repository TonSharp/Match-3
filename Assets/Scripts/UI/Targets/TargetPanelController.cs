using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VidgetConfigurator))]
public class TargetPanelController : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;

    private VidgetConfigurator vidgetSpawner;
    private TargetSpawner targetSpawner;

    private void Start()
    {
        vidgetSpawner = GetComponent<VidgetConfigurator>();
        targetSpawner = GetComponent<TargetSpawner>();
    }

    public void OnCloseButtonClicked()
    {
        GameState.EnterEditMode();
        targetPanel.SetActive(false);

        vidgetSpawner.InitializeVidgets(targetSpawner.Targets);
        CurrentLevelStats.Initialize(targetSpawner.Targets);
    }

    public void OnOpenButtonClicked()
    {
        GameState.StopEditMode();
        targetPanel.SetActive(true);
    }
}
