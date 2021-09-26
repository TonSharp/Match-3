using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TokenDestroyer : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    public VidgetConfigurator VidgetConfigurator;

    public void Destroy(List<Token> tokens)
    {
        CurrentLevelStats.UpdateTokensAfterDestroy(tokens);
        VidgetConfigurator.UpdateTokensVidgets();
        VidgetConfigurator.UpdateScoreVidget();

        foreach(var token in tokens)
        {
            HandleObstacles(GetObstaclesAroundToken(token));

            if (token is IBooster booster)
                DestroyBooster(booster, token);

            TokenPool.Remove(token);
            Destroy(token.gameObject);
        }

        TokenPool.UpdateObstaclesPool();

        VidgetConfigurator.UpdateObstaclesVidgets();
        tokens.Clear();
    }
    public void DestroyBooster(IBooster booster, Token token)
    {
        CurrentLevelStats.UpdateBoosterScore(booster);
        VidgetConfigurator.UpdateScoreVidget();

        var tokens = booster.Activate()(token.GridPos, _tilemap);

        var obst = from t in tokens where t.Type == TokenType.Ice || t.Type == TokenType.Stone select t;
        HandleObstacles(obst.ToList());

        Destroy(tokens);

        TokenPool.Remove(token);
        Destroy(token.gameObject);
    }

    private void HandleObstacles(List<Token> obstacles)
    {
        foreach(var obstacle in obstacles)
        {
            if (obstacle.LVL == 2)
                obstacle.Downgrade();
            else
            {
                TokenPool.Remove(obstacle);
                ObstaclesPool.Get().Remove(obstacle);
                Destroy(obstacle.gameObject);  
            }
        }
    }
    private List<Token> GetObstaclesAroundToken(Token token)
    {
        var obstacles = from o in GridCalculator.GetXNeighbourTokens(token.GridPos, _tilemap)
                        where o.Type == TokenType.Ice || o.Type == TokenType.Stone
                        select o;

        return obstacles.ToList();
    }
}
