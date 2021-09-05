using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private GameObject moveTargetPrefab;
    [SerializeField] private GameObject scoreTargetPrefab;
    [SerializeField] private GameObject tokenTargetPrefab;
    [SerializeField] private GameObject obstacleTargetPrefab;

    public List<ITarget> Targets { get; private set; } = new List<ITarget>();
    private bool recreation = false;

    public IEnumerable<TokenTarget> GetTokenTargets()
    {
        return from t in Targets where t is TokenTarget select (TokenTarget)t;
    }

    public void CreateMoveTarget()
    {
        foreach(var target in Targets)
            if (target is MoveTarget)
                return;

        CreateTarget(moveTargetPrefab);
    }

    public void CreateScoreTarget()
    {
        foreach (var target in Targets)
            if (target is ScoreTarget)
                return;

        CreateTarget(scoreTargetPrefab);
    }

    public void CreateTokenTarget()
    {
        if(IsAvailableType(TokensTypes.Red.ToString()))
        {
            CreateTarget(tokenTargetPrefab);
            recreation = false;
        }
        else if(!recreation)
        {
            recreation = true;
            CreateObstacleTarget();
        }
    }

    public void CreateObstacleTarget()
    {
        if (IsAvailableType(ObstaclesTypes.Stone.ToString()))
        {
            CreateTarget(obstacleTargetPrefab);
            recreation = false;
        }
        else if (!recreation)
        {
            recreation = true;
            CreateTokenTarget();
        }
    }

    public bool IsAvailableType(string type)
    {
        foreach (var target in Targets)
            if (target is ITokenTarget tTarget && tTarget.GetTokenType() == type)
                return false;

        return true;
    }

    private void CreateTarget(GameObject prefab)
    {
        var go = Instantiate(prefab);
        go.transform.SetParent(scrollRect.content.transform, false);

        if (go.TryGetComponent(out ITarget target))
            Targets.Add(target);
    }
    private void CreateTokenTarget(GameObject prefab, string type)
    {
        var go = Instantiate(prefab);
        go.transform.SetParent(scrollRect.content.transform, false);


    }

    public void DeleteTarget(ITarget target)
    {
        Targets.Remove(target);
        Destroy((target as MonoBehaviour).gameObject);
    }

    public void DeleteLastTarget()
    {
        if (Targets.Count == 0)
            return;

        ITarget last = Targets.Last();
        DeleteTarget(last);
    }
}
