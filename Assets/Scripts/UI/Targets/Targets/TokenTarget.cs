using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenTarget : MonoBehaviour, ITarget, ITokenTarget, IIntTarget
{
    [SerializeField] private Dropdown targetTypeDropdown;
    [SerializeField] private Dropdown tokenTypeDropdown;
    [SerializeField] private InputField inputField;

    [SerializeField] private string targetSpawnerName;
    private TargetSpawner spawner;

    private TargetType targetType = TargetType.Token;
    private int lastSelectedType = 0;

    private void Start()
    {
        spawner = GameObject.Find(targetSpawnerName).GetComponent<TargetSpawner>();

        var targets = Enum.GetNames(typeof(TokenTargetTypes));
        var targetsList = new List<string>(targets);

        var tokenType = Enum.GetNames(typeof(TokensTypes));
        var tokenTypeList = new List<string>(tokenType);

        targetTypeDropdown.AddOptions(targetsList);
        tokenTypeDropdown.AddOptions(tokenTypeList);
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
