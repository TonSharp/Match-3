using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleTarget : MonoBehaviour, ITarget, ITokenTarget
{
    [SerializeField] private Dropdown targetTypeDropdown;
    [SerializeField] private Dropdown tokenTypeDropdown;

    [SerializeField] private string targetSpawnerName;
    private TargetSpawner spawner;

    private TargetType targetType = TargetType.Obstacle;
    private int lastSelectedType = 1;

    private void Start()
    {
        spawner = GameObject.Find(targetSpawnerName).GetComponent<TargetSpawner>();

        var targets = Enum.GetNames(typeof(TokenTargetTypes));
        var targetsList = new List<string>(targets);

        var tokenType = Enum.GetNames(typeof(ObstaclesTypes));
        var tokenTypeList = new List<string>(tokenType);

        targetTypeDropdown.AddOptions(targetsList);
        targetTypeDropdown.value = 1;

        tokenTypeDropdown.AddOptions(tokenTypeList);
    }

    public TargetType GetTargetType()
    {
        return targetType;
    }

    public string GetTokenType()
    {
        return ((ObstaclesTypes)tokenTypeDropdown.value).ToString();
    }

    public string Serialize()
    {
        return "";
    }

    public void OnTargetTypeValueChanged()
    {
        if (targetTypeDropdown.value == 1)
            return;
        spawner.CreateTokenTarget();
        spawner.DeleteTarget(this);
    }
    public void OnTypeValueChanged()
    {
        if (spawner.IsAvailableType(tokenTypeDropdown.itemText.text))
            lastSelectedType = tokenTypeDropdown.value;
        else
            tokenTypeDropdown.value = lastSelectedType;
    }
}