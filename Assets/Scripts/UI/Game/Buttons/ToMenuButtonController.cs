using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenuButtonController : MonoBehaviour
{
    public void OnToMenuClicked()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
