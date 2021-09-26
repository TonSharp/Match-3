using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarVidget : MonoBehaviour
{
    public ScoreTarget Target;

    [SerializeField] private Text currentScoreText, targetScoreText;
    [SerializeField] private Slider progressSlider;

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            UpdateScoreVidget();
        }
    }

    public int TargetScore
    {
        get => _targetScore;
        set
        {
            _targetScore = value;
            UpdateScoreVidget();
        }
    }
    
    private int _currentScore, _targetScore;

    public void InitializeScoreValues(int current, int target)
    {
        _currentScore = current;
        _targetScore = target;

        UpdateScoreVidget();
    }

    public void UpdateScoreVidget()
    {
        currentScoreText.text = _currentScore.ToString();
        targetScoreText.text = _targetScore.ToString();

        progressSlider.maxValue = _targetScore;
        progressSlider.value = _currentScore;
    }
}
