using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesVidget : MonoBehaviour
{
    [SerializeField] private Text movesText;

    private int movesLeft;

    public int MovesLeft
    {
        get => movesLeft;
        set
        {
            movesLeft = value;
            UpdateVidget();
        }
    }

    public void InitializeVidgetValues(int moves)
    {
        movesLeft = moves;
        UpdateVidget();
    }

    public void UpdateVidget()
    {
        movesText.text = movesLeft.ToString();
    }
}
