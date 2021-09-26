using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioPanelController : MonoBehaviour
{
    [SerializeField] private GameObject audioSettingsPrefab, canvas;

    private Tuple<float, float> audioSettings = new Tuple<float, float>(0,-20);

    private GameObject currentPanel = null;
    private GameStateMode lastState;

    public void AudioSettingsClick()
    {
        EffectsPlayer.Instance().Click();

        if(currentPanel == null)
        {
            currentPanel = Instantiate(audioSettingsPrefab);
            currentPanel.transform.SetParent(canvas.transform, false);

            currentPanel.GetComponent<AudioSettingsController>().SetValues(audioSettings);

            lastState = GameState.CurrentGameState;
            GameState.Stop();
        }
        else
        {
            audioSettings = currentPanel.GetComponent<AudioSettingsController>().GetValues();

            Destroy(currentPanel);
            currentPanel = null;

            GameState.CurrentGameState = lastState;
        }
    }
}
