using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static GameStateMode CurrentGameState { get; set; } = GameStateMode.None;

    public static void EnterPlayMode() => CurrentGameState = GameStateMode.Play;
    public static void EnterEditMode() => CurrentGameState = GameStateMode.Edit;
    public static void Stop() => CurrentGameState = GameStateMode.None;
    public static void Interrupt() => CurrentGameState = GameStateMode.Interrupted;

    public static bool IsPlayMode() => CurrentGameState == GameStateMode.Play;
    public static bool IsEditMode() => CurrentGameState == GameStateMode.Edit;
    public static bool IsInterrupted() => CurrentGameState == GameStateMode.Interrupted;
}
