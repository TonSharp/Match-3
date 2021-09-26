using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelController : MonoBehaviour
{
    public StateManager stateManager;

    public void RestartClick()
    {
        stateManager.Restart();
        Destroy(gameObject);
    }
}
