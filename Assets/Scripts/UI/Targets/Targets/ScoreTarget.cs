using UnityEngine;
using UnityEngine.UI;

public class ScoreTarget : MonoBehaviour, ITarget, IIntTarget
{
    [SerializeField] private InputField inputField;

    private TargetType targetType = TargetType.ScoreGoal;

    public int GetIntValue()
    {
        return int.Parse(inputField.text);
    }

    public TargetType GetTargetType()
    {
        return targetType;
    }

    public string Serialize()
    {
        return "";
    }
}
