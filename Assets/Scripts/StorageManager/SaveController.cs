using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveController : MonoBehaviour
{
    [SerializeField] private InputField input; 

    public void Save()
    {
        string inp = input.text;

        if (inp == "")
            inp = "autosave";

        string fname = inp + ".lvl";

        using (StreamWriter stream = new StreamWriter(fname))
        {
            WriteBorders(stream);
            WriteObstacles(stream);
            WriteTargets(stream);
        }

        GameState.EnterEditMode();
        Destroy(gameObject);
    }

    private void WriteBorders(StreamWriter stream)
    {
        foreach (var border in BorderPool.Get())
        {
            stream.Write(border.x);
            stream.Write(' ');
            stream.Write(border.y);
            stream.Write(' ');
        }

        stream.Write('\n');
    }

    private void WriteObstacles(StreamWriter stream)
    {
        foreach(var ob in ObstaclesBackupPool.Get())
        {
            var token = ob.Item1.GetComponent<Token>();
            int type, lvl;

            type = (int)token.Type;
            lvl = token.LVL;

            stream.Write($"{type} {lvl} {ob.Item2.x} {ob.Item2.y} ");
        }

        stream.Write('\n');
    }

    private void WriteTargets(StreamWriter stream)
    {
        foreach (var target in TargetsPool.Targets)
        {
            stream.Write((int)target.GetTargetType());
            stream.Write(' ');

            if (target is ScoreTarget || target is MoveTarget)
                stream.Write($"{(target as IIntTarget).GetIntValue()} ");

            else
            {
                if (target is TokenTarget tt)
                {
                    Enum.TryParse((target as ITokenTarget).GetTokenType(), out TokensTypes type);
                    stream.Write($"{tt.tokenTypeDropdown.value} {(target as IIntTarget).GetIntValue()} ");
                }
                else
                {
                    Enum.TryParse((target as ITokenTarget).GetTokenType(), out ObstaclesTypes type);
                    stream.Write($"{(int)type } ");
                }
            }
        }
    }

}