using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class S_AudioSettings : MonoBehaviour{
    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public void Start(){
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value){
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("Master", dB);
    }

    public void SetMusicVolume(float value){
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("Music", dB);
    }

    public void SetSFXVolume(float value){
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("SFX", dB);
    }
}
