using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VidgetConfigurator : MonoBehaviour
{
    [SerializeField] private GameObject redPrefab, greenPrefab, bluePrefab, yellowPrefab, pinkPrefab, stonePrefab, icePrefab;
    [SerializeField] private Transform canvasTargetsTransform;

    [Space]
    [SerializeField] private GameObject scoreBar;

    [Space]
    [SerializeField] private GameObject moves;

    private List<TokenVidget> tokenVidgets = new List<TokenVidget>();
    private MovesVidget movesVidget;
    private ProgressBarVidget xpVidget;

    public void InitializeVidgets(List<ITarget> targets)
    {
        tokenVidgets.Clear();
        movesVidget = null;
        xpVidget = null;

        scoreBar.SetActive(false);
        moves.SetActive(false);
        DestroyExistingVidgets();

        var tokenTargets = from t in targets where t is TokenTarget select (TokenTarget)t;
        ConfigureTokenTargets(tokenTargets);

        var obstacleTargets = from t in targets where t is ObstacleTarget select (ObstacleTarget)t;
        ConfigureObstacleTargets(obstacleTargets);

        var scoreTarget = from t in targets where t is ScoreTarget select (ScoreTarget)t;
        if (scoreTarget.Count() > 0)
            ConfigureScoreTarget(scoreTarget.ElementAt(0));

        var movesTarget = from t in targets where t is MoveTarget select (MoveTarget)t;
        if (movesTarget.Count() > 0)
            ConfigureMovesTarget(movesTarget.ElementAt(0));
    }

    private void ConfigureTokenTargets(IEnumerable<TokenTarget> targets)
    {
        TokenVidget vidget;

        foreach (var target in targets)
        {
            var go = Instantiate(GetPrefabByTokenType(target.GetTokenType()));
            go.transform.SetParent(canvasTargetsTransform, false);

            vidget = go.GetComponent<TokenVidget>();
            vidget.InitializeVidgetValues(0, target.GetIntValue());

            tokenVidgets.Add(vidget);
        }
    }
    private void ConfigureObstacleTargets(IEnumerable<ObstacleTarget> targets)
    {
        foreach(var target in targets)
        {
            var go = Instantiate(GetPrefabByObstacleType(target.GetTokenType()));
            go.transform.SetParent(canvasTargetsTransform, false);
        }
    }
    private void ConfigureScoreTarget(ScoreTarget target)
    {
        scoreBar.SetActive(true);

        var vidget = scoreBar.GetComponent<ProgressBarVidget>();
        vidget.InitializeScoreValues(0, target.GetIntValue());

        xpVidget = vidget;
    }
    private void ConfigureMovesTarget(MoveTarget target)
    {
        moves.SetActive(true);

        var vidget = moves.GetComponent<MovesVidget>();
        vidget.InitializeVidgetValues(target.GetIntValue());

        movesVidget = vidget;
    }

    public void DestroyExistingVidgets()
    {
        for (int i = 0; i < canvasTargetsTransform.childCount; i++)
        {
            var go = canvasTargetsTransform.GetChild(i);
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
        xpVidget.CurrentScore = CurrentLevelStats.CurrentXP;
    }
    public void UpdateMovesVidget()
    {
        movesVidget.MovesLeft = CurrentLevelStats.AvailableMoves - CurrentLevelStats.UsedMoves;
    }
    public void UpdateTokensVidgets()
    {
        foreach(var vidget in tokenVidgets)
        {
            switch(vidget.TokenType)
            {
                case TokensTypes.Blue:
                    vidget.CurrentScore = CurrentLevelStats.BlueDestoyed;
                    break;
                case TokensTypes.Red:
                    vidget.CurrentScore = CurrentLevelStats.RedDestoyed;
                    break;
                case TokensTypes.Green:
                    vidget.CurrentScore = CurrentLevelStats.GreenDestoyed;
                    break;
                case TokensTypes.Yellow:
                    vidget.CurrentScore = CurrentLevelStats.YellowDestoyed;
                    break;
                case TokensTypes.Pink:
                    vidget.CurrentScore = CurrentLevelStats.PinkDestoyed;
                    break;
            }
        }
    }
}
