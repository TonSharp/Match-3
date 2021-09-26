using UnityEngine;
using UnityEngine.UI;

public class MoveTarget : MonoBehaviour, ITarget, IIntTarget
{
    public bool IsReady { get; set; } = false;

    [SerializeField] public InputField inputField;

    private TargetType targetType = TargetType.MoveLimit;

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
