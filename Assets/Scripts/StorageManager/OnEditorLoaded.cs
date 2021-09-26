using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEditorLoaded : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;

    private void Start()
    {
        if(SceneLoadingMode.IsCreating)
            gameObject.GetComponent<LoadController>().Load(SceneLoadingMode.LevelName);

        stateManager.StartEditor();
    }
}
