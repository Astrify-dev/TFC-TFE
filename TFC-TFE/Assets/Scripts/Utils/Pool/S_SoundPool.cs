using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoundPool : SingletonBehaviour<S_SoundPool>{
    private Pool<AudioSource> _audioSourcePool = new Pool<AudioSource>("Prefabs/Audio/AudioSource");

    protected override void OnInstanceDestroyed(){
        base.OnInstanceDestroyed();
        StopAllCoroutines();
        _audioSourcePool.ClearPool();
    }


    public AudioSource PlaySound(SoundSystem data, Vector3 position){
        AudioSource poolObject = PlaySound(data, true);
        poolObject.transform.position = position;
        return poolObject;
    }
    public AudioSource PlaySound(SoundSystem data){
        AudioSource poolObject = PlaySound(data, false);
        poolObject.transform.position = Vector3.zero;
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
        source.loop = false;

        source.spatialize = spatialized;
        source.spatialBlend = spatialized ? 1 : 0;
        source.ignoreListenerPause = data.affectedByTimescale;
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
