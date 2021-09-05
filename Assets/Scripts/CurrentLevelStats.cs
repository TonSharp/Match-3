using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentLevelStats
{
    public static int CurrentXP, XPTarget;
    public static int AvailableMoves, UsedMoves;

    public static int RedDestoyed, RedTarget;
    public static int GreenDestoyed, GreenTarget;
    public static int BlueDestoyed, BlueTarget;
    public static int PinkDestoyed, PinkTarget;
    public static int YellowDestoyed, YellowTarget;

    public static void Reset()
    {
        CurrentXP = 0;
        XPTarget = 0;
        AvailableMoves = 0;
        UsedMoves = 0;
        RedDestoyed = 0;
        RedTarget = 0;
        GreenDestoyed= 0;
        GreenTarget = 0;
        BlueDestoyed = 0;
        BlueTarget = 0;
        PinkDestoyed = 0;
        PinkTarget = 0;
        YellowDestoyed = 0;
        YellowTarget = 0;
    }

    public static void Initialize(List<ITarget> targets)
    {
        Reset();

        foreach(var target in targets)
        {
            if (target is TokenTarget tokenTarget)
                InitTokenTarget(tokenTarget);
            else if (target is MoveTarget moveTarget)
                InitMovesTarget(moveTarget);
            else if (target is ScoreTarget scoreTarget)
                InitScoreTarget(scoreTarget);
        }
    }

    private static void InitScoreTarget(ScoreTarget target)
    {
        XPTarget = target.GetIntValue();
    }
    private static void InitMovesTarget(MoveTarget target)
    {
        AvailableMoves = target.GetIntValue();
    }
    private static void InitTokenTarget(TokenTarget target)
    {
        var type = Enum.Parse(typeof(TokensTypes), target.GetTokenType());
        var val = target.GetIntValue();

        switch (type)
        {
            case TokensTypes.Blue:
                BlueTarget = val;
                break;

            case TokensTypes.Green:
                GreenTarget = val;
                break;

            case TokensTypes.Pink:
                PinkTarget = val;
                break;

            case TokensTypes.Red:
                RedTarget = val;
                break;

            case TokensTypes.Yellow:
                YellowTarget = val;
                break;
        }
    }

    public static void UpdateTokensAfterDestroy(List<Token> destroyed)
    {
        foreach(var token in destroyed)
        {
            switch(token.Type)
            {
                case TokenType.Blue:
                    BlueDestoyed++;
                    break;

                case TokenType.Green:
                    GreenDestoyed++;
                    break;

                case TokenType.Pink:
                    PinkDestoyed++;
                    break;

                case TokenType.Red:
                    RedDestoyed++;
                    break;

                case TokenType.Yellow:
                    YellowDestoyed++;
                    break;
            }

            CurrentXP += 10;
        }
    }
    public static void UpdateBoosterScore(IBooster booster)
    {
        if (booster is Rocket)
            CurrentXP += 30;
        else
            CurrentXP += 50;
    }
}
