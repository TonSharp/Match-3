using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightButtonsController : MonoBehaviour
{
    [SerializeField] private AnimationClip swipeOut, swipeIn;

    private Animation animation;

    void Start()
    {
        animation = GetComponent<Animation>();
    }

    public void OnPointerEnter()
    {
        EffectsPlayer.Instance().Swipe();
        animation.Play(swipeOut.name);
    }
    public void OnPointerExit()
    {
        animation.Play(swipeIn.name);
    }
}
