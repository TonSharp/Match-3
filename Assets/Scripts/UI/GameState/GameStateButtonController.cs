using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateButtonController : MonoBehaviour
{
    [SerializeField] private StateManager manager;
    [SerializeField] Sprite playSprite, stopSprite;

    private Image button;
    private bool isStopMode = true;

    private void Start()
    {
        button = GetComponent<Image>();
    }

    public void Reset()
    {
        isStopMode = true;
        button.sprite = playSprite;
    }

    public void StateChanged()
    {
        if (GameState.CurrentGameState == GameStateMode.Interrupted)
            return;

        if(isStopMode)
        {
            button.sprite = stopSprite;
            manager.StartGame();
        }
        else
        {
            button.sprite = playSprite;
            manager.StopEditGame();
        }

        isStopMode = !isStopMode;
    }
}
