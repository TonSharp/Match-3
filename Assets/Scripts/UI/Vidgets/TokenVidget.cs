using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenVidget : MonoBehaviour
{
    public TokensTypes TokenType;

    [SerializeField] private Text targetText;

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            UpdateVidgetText();
        }
    }
    public int TargetScore
    {
        get => _targetScore;
        set
        {
            _targetScore = value;
            UpdateVidgetText();
        }
    }

    private int _currentScore, _targetScore;

    public void InitializeVidgetValues(int currentScore, int targetScore)
    {
        _currentScore = currentScore;
        _targetScore = targetScore;

        UpdateVidgetText();
    }

    public void UpdateVidgetText()
    {
        targetText.text = $"{_currentScore} / {_targetScore}";
    }
}
