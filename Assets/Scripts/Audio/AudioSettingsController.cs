using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsController : MonoBehaviour
{
    [SerializeField] private Slider effects, music;
    [SerializeField] private Toggle musicTogle, effectsTogle;
    [SerializeField] private AudioMixer mixer;

    public void SetEffectsValue(float val) => effects.value = val;
    public void SetMusicValue(float val) => music.value = val;

    public Tuple<float, float> GetValues() => new Tuple<float, float>(effects.value, music.value);
    public void SetValues(Tuple<float, float> values)
    {
        SetEffectsValue(values.Item1);
        SetMusicValue(values.Item2);
    }

    public void OnMusicLevelChanged()
    {
        if(musicTogle.isOn)
            mixer.SetFloat("Music", music.value);
    }
    public void OnEffectsLevelChanged()
    {
        if (effectsTogle.isOn)
            mixer.SetFloat("Effects", effects.value);
    }

    public void OnIsMusicChanged()
    {
        if (musicTogle.isOn)
            OnMusicLevelChanged();
        else
            mixer.SetFloat("Music", -80);
    }
    public void OnIsEffectsChanged()
    {
        if (effectsTogle.isOn)
            OnEffectsLevelChanged();
        else
            mixer.SetFloat("Effects", -80);
    }
}
