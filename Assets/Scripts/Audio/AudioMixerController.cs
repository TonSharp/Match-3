using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixerController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> audios;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = audios[Random.Range(0, audios.Count)];
            audioSource.Play();
        }
    }
}
