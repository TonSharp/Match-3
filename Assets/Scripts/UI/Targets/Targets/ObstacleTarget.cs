using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleTarget : MonoBehaviour, ITarget, ITokenTarget
{
    public bool IsReady { get; set; } = false;

    [SerializeField] public Dropdown targetTypeDropdown;
    [SerializeField] public Dropdown tokenTypeDropdown;

    [SerializeField] private string targetSpawnerName;

    private TargetSpawner spawner;

    private TargetType targetType = TargetType.Obstacle;
    private int lastSelectedType = 1;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        spawner = GameObject.Find(targetSpawnerName).GetComponent<TargetSpawner>();

        targetTypeDropdown.ClearOptions();
        tokenTypeDropdown.ClearOptions();

        //var targets = Enum.GetNames(typeof(TokenTargetTypes));
        var rusTargetsList = new List<string>(Localization.RussianTokenTargetTypes.Values);
        //var targetsList = new List<string>(targets);

        //var tokenType = Enum.GetNames(typeof(ObstaclesTypes));
        var rusObstTypes = new List<string>(Localization.RussianObstaclesTypes.Values);
        //var tokenTypeList = new List<string>(tokenType);

        targetTypeDropdown.AddOptions(rusTargetsList);
        targetTypeDropdown.value = 1;

        tokenTypeDropdown.AddOptions(rusObstTypes);
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