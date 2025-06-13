using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class S_SoundManager : MonoBehaviour
{
    [SerializeField] private SoundSystem MusicAmbiance;
    [SerializeField] private AudioMixer _audioMixer;

    private void Start(){
        if (MusicAmbiance != null){
            MusicAmbiance.Play();
        }

        if (!PlayerPrefs.HasKey("VMaster")) StartSetValue(0.8f);
        else SetValue();
    }

    private void SetValue()
    {
        SetSongValue("Master", PlayerPrefs.GetFloat("VMaster"));
        SetSongValue("Music", PlayerPrefs.GetFloat("VMusic"));
        SetSongValue("SFX", PlayerPrefs.GetFloat("VSFX"));

        PlayerPrefs.Save();
    }

    private void StartSetValue(float Value)
    {

        SetSongValue("Master", Value);
        SetSongValue("Music", Value);
        SetSongValue("SFX", Value);

        PlayerPrefs.Save();
    }

    private void SetSongValue(string Mixer, float value)
    {
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        _audioMixer.SetFloat(Mixer, dB);
        PlayerPrefs.SetFloat("V" + Mixer, value);
    }

}
