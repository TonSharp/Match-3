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

    private bool recreation = false;

    public IEnumerable<TokenTarget> GetTokenTargets()
    {
        return from t in TargetsPool.Targets where t is TokenTarget select (TokenTarget)t;
    }

    public ITarget CreateMoveTarget()
    {
        foreach(var target in TargetsPool.Targets)
        {
            if (target is MoveTarget)
            {
                EffectsPlayer.Instance().Error();
                return null;
            }
        }
            
        var go = CreateTarget(moveTargetPrefab);

        return go;
    }

    public ITarget CreateScoreTarget()
    {
        foreach (var target in TargetsPool.Targets)
        {
            if (target is ScoreTarget)
            {
                EffectsPlayer.Instance().Error();
                return null;
            } 
        }
            

        return CreateTarget(scoreTargetPrefab);
    }

    public ITarget CreateTokenTarget()
    {
        if(IsAvailableType(TokensTypes.Red.ToString()))
        {
            recreation = false;

            return CreateTarget(tokenTargetPrefab);
        }
        else if(!recreation)
        {
            recreation = true;
            return CreateObstacleTarget();
        }

        EffectsPlayer.Instance().Error();
        return null;
    }

    public ITarget CreateCustomTokenTarget(TokensTypes type)
    {
        if (IsAvailableType(type.ToString()))
        {
            var target = (TokenTarget)CreateTarget(tokenTargetPrefab);
            target.tokenTypeDropdown.value = (int)type;

            return target;
        }

        EffectsPlayer.Instance().Error();
        return null;
    }

    public ITarget CreateObstacleTarget()
    {
        if (IsAvailableType(ObstaclesTypes.Stone.ToString()))
        {
            recreation = false;
            return CreateTarget(obstacleTargetPrefab);
        }
        else if (!recreation)
        {
            recreation = true;
            return CreateTokenTarget();
        }

        EffectsPlayer.Instance().Error();
        return null;
    }

    public bool IsAvailableType(string type)
    {
        foreach (var target in TargetsPool.Targets)
            if (target is ITokenTarget tTarget && tTarget.GetTokenType() == type)
                return false;

        return true;
    }

    private ITarget CreateTarget(GameObject prefab)
    {
        var go = Instantiate(prefab);
        go.transform.SetParent(scrollRect.content.transform, false);

        if (go.TryGetComponent(out ITarget target))
        {
            TargetsPool.Targets.Add(target);
            return target;
        }

        EffectsPlayer.Instance().Error();
        return null;
    }

    public void DeleteTarget(ITarget target)
    {
        TargetsPool.Targets.Remove(target);
        Destroy((target as MonoBehaviour).gameObject);
    }

    public void DeleteLastTarget()
    {
        if (TargetsPool.Targets.Count == 0)
            return;

        ITarget last = TargetsPool.Targets.Last();
        DeleteTarget(last);
    }
}
