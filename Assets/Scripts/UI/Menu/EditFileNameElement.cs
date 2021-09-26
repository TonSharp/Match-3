using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditFileNameElement : MonoBehaviour
{
    [SerializeField] private Text fNameText;
    [SerializeField] private Button loadButton;

    public void SetName(string fName)
    {
        fNameText.text = fName;
    }

    public void OnDeleteButtonClicked()
    {
        File.Delete(fNameText.text);
        Destroy(gameObject);
    }

    public void SetLoadCallback(UnityAction<string> loadCallback)
    {
        Button.ButtonClickedEvent clickedEvent = new Button.ButtonClickedEvent();
        clickedEvent.AddListener(() => { loadCallback(fNameText.text); });

        loadButton.onClick = clickedEvent;
    }
}
