using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    [SerializeField] Toggle musicTogle, effectsTogle;
    [SerializeField] Slider musicSlider, effectsSlider;
    [SerializeField] private AudioMixer mixer;

    public void Start()
    {
        musicSlider.value = AudioLevelsStore.MusicLevel;
        effectsSlider.value = AudioLevelsStore.EffectsLevel;

        musicTogle.isOn = AudioLevelsStore.IsMusicOn;
        effectsTogle.isOn = AudioLevelsStore.IsEffectsOn;
    }

    public void OnMusicLevelChanged()
    {
        AudioLevelsStore.MusicLevel = musicSlider.value;
        if (musicTogle.isOn)
            mixer.SetFloat("Music", AudioLevelsStore.MusicLevel);
    }
    public void OnEffectsLevelChanged()
    {
        AudioLevelsStore.EffectsLevel = effectsSlider.value;
        if (effectsTogle.isOn)
            mixer.SetFloat("Effects", AudioLevelsStore.EffectsLevel);
    }

    public void OnMusicStateChanged()
    {
        if (musicTogle.isOn)
        {
            mixer.SetFloat("Music", AudioLevelsStore.MusicLevel);
            AudioLevelsStore.IsMusicOn = true;
        }
        else
        {
            mixer.SetFloat("Music", -80);
            AudioLevelsStore.IsMusicOn = false;
        }

        EffectsPlayer.Instance().Click();
    }
    public void OnEffectsStateChanged()
    {
        if (effectsTogle.isOn)
        {
            mixer.SetFloat("Effects", AudioLevelsStore.EffectsLevel);
            AudioLevelsStore.IsEffectsOn = true;
        }
        else
        {
            mixer.SetFloat("Effects", -80);
            AudioLevelsStore.IsEffectsOn = false;
        }

        EffectsPlayer.Instance().Click();
    }
}
