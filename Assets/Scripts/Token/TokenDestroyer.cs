using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TokenDestroyer : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private VidgetConfigurator vidgetConfigurator;

    public void Destroy(List<Token> tokens)
    {
        CurrentLevelStats.UpdateTokensAfterDestroy(tokens);
        vidgetConfigurator.UpdateTokensVidgets();
        vidgetConfigurator.UpdateScoreVidget();

        foreach(var token in tokens)
        {
            HandleObstacles(GetObstaclesAroundToken(token));

            TokenPool.Remove(token);
            Destroy(token.gameObject);
        }

        tokens.Clear();
    }
    public void DestroyBooster(IBooster booster, Token token)
    {
        CurrentLevelStats.UpdateBoosterScore(booster);
        vidgetConfigurator.UpdateScoreVidget();

        var tokens = booster.Activate()(token.GridPos, _tilemap);

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
