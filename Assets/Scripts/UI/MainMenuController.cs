using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPrefab;
    [SerializeField] private GameObject editPrefab, playPrefab;

    [SerializeField] private Dropdown editLevelDropdown, loadLevelDropdown;

    public void Awake()
    {
        PoolsManager.ClearPools();
    }

    public void PlayClick()
    {
        EffectsPlayer.Instance().Click();

        mainMenuPrefab.SetActive(false);

        loadLevelDropdown.ClearOptions();
        loadLevelDropdown.AddOptions(LoadFileNames());

        playPrefab.SetActive(true);
    }
    public void EditorClick()
    {
        EffectsPlayer.Instance().Click();

        mainMenuPrefab.SetActive(false);

        editLevelDropdown.ClearOptions();
        editLevelDropdown.AddOptions(LoadFileNames());

        editPrefab.SetActive(true);
    }
    public void ExitClick()
    {
        EffectsPlayer.Instance().Click();

        Application.Quit();
    }

    public void EditPlayClick()
    {
        EffectsPlayer.Instance().Click();

        if (editLevelDropdown.options.Count == 0)
            return;

        SceneLoadingMode.IsCreating = true;
        SceneLoadingMode.LevelName = editLevelDropdown.options[editLevelDropdown.value].text;

        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
    }
    public void EditCreateClick()
    {
        EffectsPlayer.Instance().Click();

        SceneLoadingMode.IsCreating = false;

        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
    }

    public void GamePlayClick()
    {
        EffectsPlayer.Instance().Click();

        if (loadLevelDropdown.options.Count == 0)
            return;

        SceneLoadingMode.IsCreating = false;
        SceneLoadingMode.LevelName = loadLevelDropdown.options[loadLevelDropdown.value].text;

        SceneManager.LoadScene("PlayMode", LoadSceneMode.Single);
    }

    public void BackToMenuClick()
    {
        EffectsPlayer.Instance().Click();

        editPrefab.SetActive(false);
        playPrefab.SetActive(false);
        mainMenuPrefab.SetActive(true);
    }

    private List<string> LoadFileNames()
    {
        EffectsPlayer.Instance().Click();

        List<string> names = new List<string>();
        foreach (var f in Directory.GetFiles("./"))
        {
            FileInfo fnfo = new FileInfo(f);

            if (fnfo.Extension == ".lvl")
                names.Add(f.TrimStart(new char[] { '.', '/' }));
        }

        return names;
    }
}
