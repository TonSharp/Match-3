using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDialogController : MonoBehaviour
{
    [SerializeField] private GameObject savePanel, canvas;

    public void ShowSaveDialog()
    {
        if (GameState.IsPlayMode())
            return;

        var go = Instantiate(savePanel);
        go.transform.SetParent(canvas.transform, false);

        GameState.Interrupt();
    }
}
