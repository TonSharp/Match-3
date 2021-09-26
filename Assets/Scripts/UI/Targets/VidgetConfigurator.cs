using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VidgetConfigurator : MonoBehaviour
{
    [SerializeField] private GameObject redPrefab, greenPrefab, bluePrefab, yellowPrefab, pinkPrefab, stonePrefab, icePrefab;
    [SerializeField] private Transform targetHolder;

    [Space]
    [SerializeField] private GameObject scoreBar;

    [Space]
    [SerializeField] private GameObject moves;

    private List<TokenVidget> tokenVidgets = new List<TokenVidget>();
    private List<TokenVidget> obstacleVidgets = new List<TokenVidget>();
    private MovesVidget movesVidget;
    private ProgressBarVidget xpVidget;

    private void Start()
    {
        InitializeVidgets();
    }

    public void InitializeVidgets()
    {
        tokenVidgets.Clear();
        obstacleVidgets.Clear();

        movesVidget = null;
        xpVidget = null;

        scoreBar.SetActive(false);
        moves.SetActive(false);
        DestroyExistingVidgets();

        var tokenTargets = from t in TargetsPool.Targets where t is TokenTarget select (TokenTarget)t;
        ConfigureTokenTargets(tokenTargets);

        var obstacleTargets = from t in TargetsPool.Targets where t is ObstacleTarget select (ObstacleTarget)t;
        ConfigureObstacleTargets(obstacleTargets);

        var scoreTarget = from t in TargetsPool.Targets where t is ScoreTarget select (ScoreTarget)t;
        if (scoreTarget.Count() > 0)
            ConfigureScoreTarget(scoreTarget.ElementAt(0));

        var movesTarget = from t in TargetsPool.Targets where t is MoveTarget select (MoveTarget)t;
        if (movesTarget.Count() > 0)
            ConfigureMovesTarget(movesTarget.ElementAt(0));
    }

    private void ConfigureTokenTargets(IEnumerable<TokenTarget> targets)
    {
        TokenVidget vidget;

        foreach (var target in targets)
        {
            var go = Instantiate(GetPrefabByTokenType(target.GetTokenType()));
            go.transform.SetParent(targetHolder, false);

            vidget = go.GetComponent<TokenVidget>();
            vidget.InitializeVidgetValues(0, target.GetIntValue());

            vidget.Target = target;

            tokenVidgets.Add(vidget);
        }
    }
    private void ConfigureObstacleTargets(IEnumerable<ObstacleTarget> targets)
    {
        foreach(var target in targets)
        {
            var go = Instantiate(GetPrefabByObstacleType(target.GetTokenType()));
            go.transform.SetParent(targetHolder, false);

            var vidget = go.GetComponent<TokenVidget>();
            vidget.Target = target;

            if (vidget.ObstaclesTypes == ObstaclesTypes.Ice)
                vidget.SetValue(ObstaclesPool.GetIceCount());
            else
                vidget.SetValue(ObstaclesPool.GetStoneCount());

            obstacleVidgets.Add(vidget);
        }
    }
    private void ConfigureScoreTarget(ScoreTarget target)
    {
        scoreBar.SetActive(true);

        var vidget = scoreBar.GetComponent<ProgressBarVidget>();
        vidget.InitializeScoreValues(0, target.GetIntValue());

        vidget.Target = target;

        xpVidget = vidget;
    }
    private void ConfigureMovesTarget(MoveTarget target)
    {
        moves.SetActive(true);

        var vidget = moves.GetComponent<MovesVidget>();
        vidget.InitializeVidgetValues(target.GetIntValue());

        vidget.Target = target;

        movesVidget = vidget;
    }

    public void DestroyExistingVidgets()
    {
        for (int i = 0; i < targetHolder.childCount; i++)
        {
            var go = targetHolder.GetChild(i);
            Destroy(go.gameObject);
        }
    }

    public GameObject GetPrefabByTokenType(string type)
    {
        var tokenType = Enum.Parse(typeof(TokensTypes), type);

        switch(tokenType)
        {
            case TokensTypes.Blue:
                return bluePrefab;
            case TokensTypes.Green:
                return greenPrefab;
            case TokensTypes.Pink:
                return pinkPrefab;
            case TokensTypes.Red:
                return redPrefab;
            case TokensTypes.Yellow:
                return yellowPrefab;
            default:
                return null;
        }
    }
    public GameObject GetPrefabByObstacleType(string type)
    {
        var obstacleType = Enum.Parse(typeof(ObstaclesTypes), type);

        switch(obstacleType)
        {
            case ObstaclesTypes.Ice:
                return icePrefab;
            case ObstaclesTypes.Stone:
                return stonePrefab;
            default:
                return null;
        }
    }

    public void UpdateScoreVidget()
    {
        if (xpVidget == null)
            return;

        xpVidget.CurrentScore = CurrentLevelStats.CurrentXP;

        if (CurrentLevelStats.CurrentXP >= CurrentLevelStats.XPTarget)
        {
            xpVidget.Target.IsReady = true;
            xpVidget.CurrentScore = CurrentLevelStats.XPTarget;
        }
    }
    public void UpdateMovesVidget()
    {
        if (movesVidget == null)
            return;

        movesVidget.MovesLeft = CurrentLevelStats.AvailableMoves - CurrentLevelStats.UsedMoves;

        if(movesVidget.MovesLeft == 0)
        {
            movesVidget.Target.IsReady = true;
        }
    }
    public void UpdateObstaclesVidgets()
    {
        var iceCount = ObstaclesPool.GetIceCount();
        var stoneCount = ObstaclesPool.GetStoneCount();

        foreach(var vidget in obstacleVidgets)
        {
            switch(vidget.ObstaclesTypes)
            {
                case ObstaclesTypes.Ice:
                    vidget.SetValue(iceCount);

                    if (iceCount == 0)
                        vidget.Target.IsReady = true;
                    break;

                case ObstaclesTypes.Stone:
                    vidget.SetValue(stoneCount);

                    if (stoneCount == 0)
                        vidget.Target.IsReady = true;
                    break;
            }
        }
    }
    public void UpdateTokensVidgets()
    {
        foreach(var vidget in tokenVidgets)
        {
            switch(vidget.TokenType)
            {
                case TokensTypes.Blue:
                    vidget.CurrentScore = CurrentLevelStats.BlueDestoyed;
                    if (CurrentLevelStats.BlueDestoyed >= CurrentLevelStats.BlueTarget)
                    {
                        vidget.Target.IsReady = true;
                        vidget.CurrentScore = CurrentLevelStats.BlueTarget;
                    }
                        
                    break;
                case TokensTypes.Red:
                    vidget.CurrentScore = CurrentLevelStats.RedDestoyed;
                    if (CurrentLevelStats.RedDestoyed >= CurrentLevelStats.RedTarget)
                    {
                        vidget.Target.IsReady = true;
                        vidget.CurrentScore = CurrentLevelStats.RedTarget;
                    }
                    break;
                case TokensTypes.Green:
                    vidget.CurrentScore = CurrentLevelStats.GreenDestoyed;
                    if (CurrentLevelStats.GreenDestoyed >= CurrentLevelStats.GreenTarget)
                    {
                        vidget.Target.IsReady = true;
                        vidget.CurrentScore = CurrentLevelStats.GreenTarget;
                    }
                    break;
                case TokensTypes.Yellow:
                    vidget.CurrentScore = CurrentLevelStats.YellowDestoyed;
                    if (CurrentLevelStats.YellowDestoyed >= CurrentLevelStats.YellowTarget)
                    {
                        vidget.Target.IsReady = true;
                        vidget.CurrentScore = CurrentLevelStats.YellowTarget;
                    }
                    break;
                case TokensTypes.Pink:
                    vidget.CurrentScore = CurrentLevelStats.PinkDestoyed;
                    if (CurrentLevelStats.PinkDestoyed >= CurrentLevelStats.PinkTarget)
                    {
                        vidget.Target.IsReady = true;
                        vidget.CurrentScore = CurrentLevelStats.PinkTarget;
                    }
                    break;
            }
        }
    }
}
