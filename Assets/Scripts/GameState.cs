using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static bool IsPlayMode { get; private set; } = false;
    public static bool IsEditMode { get; private set; } = true;

    public static void EnterPlayMode()
    {
        IsPlayMode = true;
    }
    public static void StopPlayMode()
    {
        IsPlayMode = false;
    }

    public static void EnterEditMode()
    {
        IsEditMode = true;
    }
    public static void StopEditMode()
    {
        IsEditMode = false;
    }
}
