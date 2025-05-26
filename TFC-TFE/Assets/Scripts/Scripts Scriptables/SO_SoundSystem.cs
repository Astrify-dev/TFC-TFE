using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSystem", menuName = "ScriptableObjects/SoundSystem", order = 1)]

public class SoundSystem : ScriptableObject{
    [field: SerializeField] private List<AudioClip> _sounds;

    [field: SerializeField] public float FadeDuration { get; private set; }

    [field: SerializeField, MinMaxSlider(0.1f, 1f)] private Vector2 _volume = new Vector2(0f, 1f);
    [field: SerializeField] private AnimationCurve _randomVolumeCurve = AnimationCurve.Linear(0, 1, 1, 1);
    [field: SerializeField, MinMaxSlider(-3f, 3f)] private Vector2 _pitch = new Vector2(-3f, 3f);
    [field: SerializeField] private AnimationCurve _randomPitchCurve = AnimationCurve.Linear(0, 1, 1, 1);
    [field: SerializeField] public float PitchOnSlow { get; private set; } = 0.5f;
    [field: SerializeField] public AudioMixerGroup mixerGroup { get; private set; }
    [field: SerializeField] public bool Loop { get; private set; } = false;

    public AudioClip getRandomSong => _sounds[Random.Range(0, _sounds.Count)];
    public float getRandomVolume => Mathf.Lerp(_volume.x, _volume.y, _randomVolumeCurve.Evaluate(UnityEngine.Random.value));
    public float getRandomPitch => Mathf.Lerp(_pitch.x, _pitch.y, _randomPitchCurve.Evaluate(UnityEngine.Random.value));

    public AudioSource Play() => S_SoundPool.instance.PlaySound(this);

    public AudioSource Play(Vector3 position) => S_SoundPool.instance.PlaySound(this, position);

}

