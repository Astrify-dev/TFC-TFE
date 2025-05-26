using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoundPool : SingletonBehaviour<S_SoundPool>{
    private Pool<AudioSource> _audioSourcePool = new Pool<AudioSource>("Prefabs/Audio/AudioSource");
    public static Action<bool> OnPitchChange;
    private float _defaultPitch = 1f;
    private float _SlowPitch = 0.5f;
    protected override void OnInstanceDestroyed(){
        base.OnInstanceDestroyed();
        StopAllCoroutines();
        _audioSourcePool.ClearPool();
    }

    public static void SetPitch(bool end){
        OnPitchChange?.Invoke(end);
    }

    private void OnEnable()
    {
        OnPitchChange += ChangePitch;
    }

    private void OnDisable()
    {
        OnPitchChange -= ChangePitch;
    }

    private void ChangePitch(bool End){
        foreach (var audioSource in _audioSourcePool.GetActiveElements()){
            if (audioSource is not null){
                if (End) 
                    audioSource.pitch = _defaultPitch;
                else
                    audioSource.pitch = _SlowPitch;
            }
        }
    }

    public AudioSource PlaySound(SoundSystem data, Vector3 position){
        AudioSource poolObject = PlaySound(data, true);
        poolObject.transform.position = position;
        return poolObject;
    }
    public AudioSource PlaySound(SoundSystem data){
        AudioSource poolObject = PlaySound(data, false);
        poolObject.transform.position = Vector3.zero;
        _SlowPitch = data.PitchOnSlow;
        return poolObject;
    }
    private AudioSource PlaySound(SoundSystem data, bool spatialized){
        AudioSource poolObject = _audioSourcePool.GetFromPool();
        SetDataOnAudioSource(poolObject, data, spatialized);
        StartCoroutine(BackToPoolRoutine(poolObject));
        return poolObject;
    }
    public static void SetDataOnAudioSource(AudioSource source, SoundSystem data, bool spatialized){
        source.clip = data.getRandomSong;
        source.volume = data.getRandomVolume;
        source.pitch = data.getRandomPitch;
        source.loop = data.Loop;

        source.spatialize = spatialized;
        source.spatialBlend = spatialized ? 1 : 0;
        source.outputAudioMixerGroup = data.mixerGroup;
    }

    private IEnumerator BackToPoolRoutine(AudioSource poolObject){
        poolObject.gameObject.SetActive(true);
        poolObject.Play();
        while (poolObject.isPlaying && poolObject.gameObject.activeSelf)
            yield return null;

        poolObject.gameObject.SetActive(false);
        _audioSourcePool.BackToPool(poolObject);
    }
}
