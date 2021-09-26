using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateButtonController : MonoBehaviour
{
    public void OnCreateClick()
    {
        EffectsPlayer.Instance().Click();
        SceneLoadingMode.IsCreating = false;

        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
    }
}
