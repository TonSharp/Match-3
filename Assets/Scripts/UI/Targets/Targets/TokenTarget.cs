using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenTarget : MonoBehaviour, ITarget, ITokenTarget, IIntTarget
{
    public bool IsReady { get; set; } = false;

    [SerializeField] public Dropdown targetTypeDropdown;
    [SerializeField] public Dropdown tokenTypeDropdown;
    [SerializeField] public InputField inputField;

    [SerializeField] private string targetSpawnerName;
    private TargetSpawner spawner;

    private TargetType targetType = TargetType.Token;
    private int lastSelectedType = 0;

    public void ChangeTokenTypeValue(int value)
    {
        tokenTypeDropdown.value = value;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        spawner = GameObject.Find(targetSpawnerName).GetComponent<TargetSpawner>();

        targetTypeDropdown.ClearOptions();
        tokenTypeDropdown.ClearOptions();

        var rusTokenTarget = new List<string>(Localization.RussianTokenTargetTypes.Values);
        //var targets = Enum.GetNames(typeof(TokenTargetTypes));
        //var targetsList = new List<string>(targets);

        var rusTokenType = new List<string>(Localization.RussianTokensTypes.Values);
        //var tokenType = Enum.GetNames(typeof(TokensTypes));
        //var tokenTypeList = new List<string>(tokenType);

        targetTypeDropdown.AddOptions(rusTokenTarget);
        tokenTypeDropdown.AddOptions(rusTokenType);
    }


    public TargetType GetTargetType()
    {
        return targetType;
    }

    public string GetTokenType()
    {
        return ((TokensTypes)tokenTypeDropdown.value).ToString();
    }

    public string Serialize()
    {
        return "";
    }

    public void OnTargetTypeValueChanged()
    {
        if (targetTypeDropdown.value == 0)
            return;

        spawner.CreateObstacleTarget();
        spawner.DeleteTarget(this);
    }
    public void OnTypeValueChanged()
    {
        if (spawner.IsAvailableType(tokenTypeDropdown.itemText.text))
            lastSelectedType = tokenTypeDropdown.value;
        else
            tokenTypeDropdown.value = lastSelectedType;
    }

    public int GetIntValue()
    {
        return int.Parse(inputField.text);
    }
}
