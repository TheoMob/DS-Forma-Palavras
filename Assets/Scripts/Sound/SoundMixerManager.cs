using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
    }

    public void SetSoundFXVolume(float level)
    {
        _audioMixer.SetFloat("soundVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
    }
}
