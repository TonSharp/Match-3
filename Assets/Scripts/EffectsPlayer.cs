using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsPlayer : MonoBehaviour
{
    private static EffectsPlayer instance = null;

    [SerializeField] private AudioClip click, error, startLevel, win, fail, swipe, bomb, rocket, match, unmatch;
    [SerializeField] private AudioSource audioSource;

    public void Click() => audioSource.PlayOneShot(click);
    public void Error() => audioSource.PlayOneShot(error);
    public void StartLevel() => audioSource.PlayOneShot(startLevel);
    public void Win() => audioSource.PlayOneShot(win);
    public void Fail() => audioSource.PlayOneShot(fail);
    public void Swipe() => audioSource.PlayOneShot(swipe);
    public void Match() => audioSource.PlayOneShot(match);
    public void UnMatch() => audioSource.PlayOneShot(unmatch);
    public void Bomb() => audioSource.PlayOneShot(bomb);
    public void Rocket() => audioSource.PlayOneShot(rocket);

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public static EffectsPlayer Instance()
    {
        return instance;
    }
}
