using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorConfigurator : MonoBehaviour
{
    private void Awake()
    {
        GameState.EnterEditMode();
    }
}
