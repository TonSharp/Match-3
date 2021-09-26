using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadEditController : MonoBehaviour
{
    [SerializeField] private GameObject scrollElementPrefab;
    [SerializeField] private RectTransform scrollContent;

    [SerializeField] private Text currentFName;

    public void Start()
    {
        var levels = MenuStorageManager.Instance().LoadLevelsNames();

        foreach (var lvl in levels)
        {
            var go = Instantiate(scrollElementPrefab);
            go.transform.SetParent(scrollContent, false);

            var element = go.GetComponent<EditFileNameElement>();

            element.SetName(lvl);
            element.SetLoadCallback(RecievedCallback);
        }
    }

    public void OnOpenButtonClicked()
    {
        EffectsPlayer.Instance().Click();

        if (MenuStorageManager.Instance().IsLevelExists(currentFName.text))
        {
            SceneLoadingMode.IsCreating = true;
            SceneLoadingMode.LevelName = currentFName.text;

            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
        }
    }

    private void RecievedCallback(string fname)
    {
        EffectsPlayer.Instance().Click();

        currentFName.text = fname;
    }
}
