using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject loadPlayLevelPanel, loadEditLevelPanel, settingsPanel;

    private static MainMenuManager instance;
    private GameObject curentLoadPlay, curentLoadEdit, curentSettings;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void OnStartLevelClick()
    {
        EffectsPlayer.Instance().Click();

        if(curentLoadPlay != null)
        {
            curentLoadPlay.GetComponentInChildren<MenuPanelController>().Destroy();
            curentLoadPlay = null;

            return;
        }

        ClearScreen();
        curentLoadPlay = Instantiate(loadPlayLevelPanel);
        curentLoadPlay.transform.SetParent(canvas.transform, false);
    }
    public void OnEditLevelClick()
    {
        EffectsPlayer.Instance().Click();

        if (curentLoadEdit != null)
        {
            curentLoadEdit.GetComponentInChildren<MenuPanelController>().Destroy();
            curentLoadEdit = null;

            return;
        }

        ClearScreen();
        curentLoadEdit = Instantiate(loadEditLevelPanel);
        curentLoadEdit.transform.SetParent(canvas.transform, false);
    }
    public void OnSettingsPanelClick()
    {
        EffectsPlayer.Instance().Click();

        if (curentSettings != null)
        {
            curentSettings.GetComponentInChildren<MenuPanelController>().Destroy();
            curentSettings = null;

            return;
        }

        ClearScreen();
        curentSettings = Instantiate(settingsPanel);
        curentSettings.transform.SetParent(canvas.transform, false);
    }
    public void OnExitClick()
    {
        EffectsPlayer.Instance().Click();

        Application.Quit();
    }

    public void ClearScreen()
    {
        if (curentLoadEdit)
        {
            curentLoadEdit.GetComponentInChildren<MenuPanelController>().Destroy();
            curentLoadEdit = null;
        }

        if (curentLoadPlay)
        {
            curentLoadPlay.GetComponentInChildren<MenuPanelController>().Destroy();
            curentLoadPlay = null;
        }

        if (curentSettings)
        {
            curentSettings.GetComponentInChildren<MenuPanelController>().Destroy();
            curentSettings = null;
        }
            
    }

    public MainMenuManager Instance()
    {
        return instance;
    }
}
