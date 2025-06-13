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


        SetValue();



    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    private void SetValue()
    {
        masterSlider.value = PlayerPrefs.GetFloat("VMaster");
        musicSlider.value = PlayerPrefs.GetFloat("VMusic");
        sfxSlider.value = PlayerPrefs.GetFloat("VSFX");
    }

    public void SetMasterVolume(float value){
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("Master", dB);
        PlayerPrefs.SetFloat("VMaster", value);
    }

    public void SetMusicVolume(float value){
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("Music", dB);
        PlayerPrefs.SetFloat("VMusic", value);
    }

    public void SetSFXVolume(float value){
        float steppedValue = Mathf.Round(value * 10f) / 10f;
        float clamped = Mathf.Max(steppedValue, 0.0001f);
        float dB = Mathf.Log10(clamped) * 20f;
        audioMixer.SetFloat("SFX", dB);
        PlayerPrefs.SetFloat("VSFX", value);
    }
}
