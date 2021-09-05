using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType
{
    Blue,
    Green,
    Red,
    Pink,
    Yellow,
    Bomb,
    Rocket,
    Ice,
    Stone,
    None
}

public static class TokenTypeCasting
{
    public static bool IsObstacle(TokenType type)
    {
        return type == TokenType.Stone || type == TokenType.Ice;
    }
    public static TokenType GetTypeByRusStr(string str)
    {
        switch(str)
        {
            case "Камень":
                return TokenType.Stone;
            case "Лёд":
                return TokenType.Ice;
            case "Красный":
                return TokenType.Red;
            case "Синий":
                return TokenType.Blue;
            case "Жёлтый":
                return TokenType.Yellow;
            case "Розовый":
                return TokenType.Pink;
            case "Зелёный":
                return TokenType.Green;
            default:
                return TokenType.None;
        }
    }
    //public static Color TypeToColor(TokenType type)
    //{
    //    switch(type)
    //    {
    //        case TokenType.Blue:
    //            break;

    //        case TokenType.Red:
    //            break;

    //        case TokenType.Pink:
    //            break;

    //        case TokenType.Green:
    //            break;

    //        case TokenType.Yellow:
    //            break;

    //        default:
    //            break;
    //    }
    //}
}
