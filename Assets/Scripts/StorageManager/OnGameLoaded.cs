using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameLoaded : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;

    private void Start()
    {
        gameObject.GetComponent<LoadController>().Load(SceneLoadingMode.LevelName);
        stateManager.StartGame();
    }
}
