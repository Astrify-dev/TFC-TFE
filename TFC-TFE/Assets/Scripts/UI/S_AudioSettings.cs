using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class S_AudioSettings : MonoBehaviour{
    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start(){
        //Charger des valeurs sauvegardées ici
    }

    public void SetMasterVolume(float value){
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value){
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value){
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}
