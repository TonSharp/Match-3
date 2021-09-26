using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameController : MonoBehaviour
{
    [SerializeField] private GameObject scrollElementPrefab;
    [SerializeField] private RectTransform scrollContent;

    [SerializeField] private Text currentFName;

    public void Start()
    {
        var levels = MenuStorageManager.Instance().LoadLevelsNames();

        foreach(var lvl in levels)
        {
            var go = Instantiate(scrollElementPrefab);
            go.transform.SetParent(scrollContent, false);

            var element = go.GetComponent<FileNameElement>();

            element.SetName(lvl);
            element.SetLoadCallback(RecievedCallback);
        }
    }

    public void OnCloseButtonClicked()
    {
        EffectsPlayer.Instance().Click();

        Destroy(gameObject);
    }

    public void OnOpenButtonClicked()
    {
        EffectsPlayer.Instance().Click();

        if (MenuStorageManager.Instance().IsLevelExists(currentFName.text))
        {
            SceneLoadingMode.IsCreating = false;
            SceneLoadingMode.LevelName = currentFName.text;

            SceneManager.LoadScene("PlayMode", LoadSceneMode.Single);
        }
    }

    private void RecievedCallback(string fname)
    {
        EffectsPlayer.Instance().Click();

        currentFName.text = fname;
    }
}
