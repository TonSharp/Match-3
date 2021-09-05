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
    public Dictionary<TokensTypes, string> RussianTokensTypes = new Dictionary<TokensTypes, string>()
    {
        { TokensTypes.Red, "Красный" },
        { TokensTypes.Green, "Зелёный" },
        { TokensTypes.Blue, "Синий" },
        { TokensTypes.Pink, "Розовый" },
        { TokensTypes.Yellow, "Жёлтый" }
    };

    public Dictionary<ObstaclesTypes, string> RussianObstaclesTypes = new Dictionary<ObstaclesTypes, string>()
    {
        {ObstaclesTypes.Ice, "Лёд" },
        {ObstaclesTypes.Stone, "Камень" }
    };

    public Dictionary<TokenTargetTypes, string> RussianTokenTargetTypes = new Dictionary<TokenTargetTypes, string>()
    {
        {TokenTargetTypes.Obstacles, "Препятствия" },
        {TokenTargetTypes.Tokens, "Токены" }
    };
}
