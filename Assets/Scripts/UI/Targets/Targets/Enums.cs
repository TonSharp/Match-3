using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TokensTypes
{
    Red,
    Green,
    Blue,
    Pink,
    Yellow
}

public enum ObstaclesTypes
{
    Stone,
    Ice
}

public enum TokenTargetTypes
{
    Tokens,
    Obstacles
}

public partial class Localization
{
    public static Dictionary<TokensTypes, string> RussianTokensTypes = new Dictionary<TokensTypes, string>()
    {
        { TokensTypes.Red, "Красный" },
        { TokensTypes.Green, "Зелёный" },
        { TokensTypes.Blue, "Синий" },
        { TokensTypes.Pink, "Розовый" },
        { TokensTypes.Yellow, "Жёлтый" }
    };

    public static Dictionary<ObstaclesTypes, string> RussianObstaclesTypes = new Dictionary<ObstaclesTypes, string>()
    {
        {ObstaclesTypes.Ice, "Камень" },
        {ObstaclesTypes.Stone, "Лёд" }
    };

    public static Dictionary<TokenTargetTypes, string> RussianTokenTargetTypes = new Dictionary<TokenTargetTypes, string>()
    {
        {TokenTargetTypes.Obstacles, "Токены" },
        {TokenTargetTypes.Tokens, "Препятствия" }
    };
}
