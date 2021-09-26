using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonsController : MonoBehaviour
{
    [SerializeField] private bool IsPlayAnimation;
    [SerializeField] private Sprite shadowButton, mainButton;
    [SerializeField] private AnimationClip swipeOutClip, swipeInClip;

    private Image img;
    private Animation animation;

    private void Start()
    {
        img = GetComponent<Image>();
        animation = GetComponent<Animation>();

        EventTrigger.Entry mouseEntry = new EventTrigger.Entry();
        EventTrigger.Entry mouseExit = new EventTrigger.Entry();

        mouseEntry.eventID = EventTriggerType.PointerEnter;
        mouseEntry.callback.AddListener((eventData) => { MouseEnter(); });

        mouseExit.eventID = EventTriggerType.PointerExit;
        mouseExit.callback.AddListener((eventData) => { MouseExit(); });

        var triger = gameObject.AddComponent<EventTrigger>();
        triger.triggers.Add(mouseEntry);
        triger.triggers.Add(mouseExit);
    }

    private void MouseEnter()
    {
        EffectsPlayer.Instance().Swipe();
        img.sprite = shadowButton;

        if (IsPlayAnimation)
            animation.Play(swipeOutClip.name);
    }
    private void MouseExit()
    {
        img.sprite = mainButton;

        if (IsPlayAnimation)
            animation.Play(swipeInClip.name);
    }
}
