using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.Rendering.DebugUI;

public class S_wallBreak : MonoBehaviour, IbreakWall
{
    public static Action OnReset;
    [SerializeField] Renderer _render;
    [SerializeField] GameObject _wallCollider;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _curveDestruct;

    [SerializeField] float _speedScreenShake = 2;
    [SerializeField] float _strengthScreenShake = 0.2f;

    [SerializeField] SoundSystem SFX_WallBreak;

    private MaterialPropertyBlock _mpb;
    private IEnumerator _coroutine;

    private void OnEnable()
    {
        OnReset += ResetWall;
    }

    private void OnDisable()
    {
        OnReset -= ResetWall;
    }

    private void Start()
    {
        _wallCollider.SetActive(true);
        _mpb = new MaterialPropertyBlock();
    }

    public void BreakWall(float VelocityX)
    {
        _wallCollider.SetActive(false);

        bool RightBreak = VelocityX > 0;

        S_controllerPlayer.Instance.CameraShake.Shake(_speedScreenShake, _strengthScreenShake);

        if (_coroutine is not null)
            StopCoroutine(_coroutine);

        _coroutine = _StartDestruct(RightBreak);
        SFX_WallBreak.Play();
        StartCoroutine(_coroutine);
    }

    private IEnumerator _StartDestruct(bool RightBreak)
    {
        float Timer = 0;
        float Value = 0f;
        _mpb.SetFloat("_SliderDirBreak", Value);
        _render.SetPropertyBlock(_mpb);

        while (Timer < 1)
        {
            Timer += Time.deltaTime * _speed;

            Value = _curveDestruct.Evaluate(Timer) * 0.45f;
            Value *= RightBreak ? 1 : -1;

            _mpb.SetFloat("_SliderDirBreak", Value);
            _render.SetPropertyBlock(_mpb);

            yield return null;
        }
        Value = 0.45f * (RightBreak ? 1 : -1);

        _mpb.SetFloat("_SliderDirBreak", Value);
        _render.SetPropertyBlock(_mpb);


    }

    private void ResetWall(){
        _wallCollider.SetActive(true);
        if(_coroutine is not null)
            StopCoroutine(_coroutine);

        _mpb.SetFloat("_SliderDirBreak", 0);
        _render.SetPropertyBlock(_mpb);

    }


}
