using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField] private AnimationClip swipeOut, swipeIn, swipeOutCursor, swipeInCursor;
    [SerializeField] private Sprite bg, shadowBg;

    private Animation animation;
    private Image image;

    void Start()
    {
        animation = GetComponent<Animation>();
        image = GetComponent<Image>();

        animation.Play(swipeOut.name);
    }

    public void MouseEnter()
    {
        EffectsPlayer.Instance().Swipe();
        animation.Play(swipeOutCursor.name);

        image.sprite = shadowBg;
    }
    public void MouseExit()
    {
        animation.Play(swipeInCursor.name);
        image.sprite = bg;
    }

    public void OnCloseButtonClicked()
    {
        EffectsPlayer.Instance().Click();
        Destroy();
    }

    public void Destroy()
    {
        animation.Play(swipeIn.name);
        Destroy(transform.parent.gameObject, swipeIn.length);
    }
}
