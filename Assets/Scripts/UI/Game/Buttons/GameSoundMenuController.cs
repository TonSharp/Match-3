using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundMenuController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanelPrefab, canvas;
    private GameObject currentSettings;

    public void OnSettingsButtonClicked()
    {
        if(currentSettings == null)
        {
            currentSettings = Instantiate(settingsPanelPrefab);
            currentSettings.transform.SetParent(canvas.transform, false);
        }
        else
        {
            currentSettings.GetComponentInChildren<MenuPanelController>().Destroy();
            currentSettings = null;
        }
    }
}
