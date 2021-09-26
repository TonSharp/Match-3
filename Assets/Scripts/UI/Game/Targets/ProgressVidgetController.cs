using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressVidgetController : MonoBehaviour
{
    [SerializeField] private Text current, target;
    [SerializeField] private Slider progressBar;

    public void SetValues(int current, int target)
    {
        this.current.text = current.ToString();
        this.target.text = target.ToString();

        progressBar.minValue = current;
        progressBar.maxValue = target;
    }

    public void SetCurrent(int value)
    {
        current.text = value.ToString();
        progressBar.value = value;
    }
}
