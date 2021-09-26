using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidgetValueController : MonoBehaviour
{
    [SerializeField] private Text valueText;

    public void SetValue(int val)
    {
        if (val < 0)
            val = 0;

        valueText.text = val.ToString();
    }
}
