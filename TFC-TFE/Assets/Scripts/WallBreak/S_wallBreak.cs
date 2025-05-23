using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_wallBreak : MonoBehaviour, IbreakWall
{
    [SerializeField] Renderer _render;
    [SerializeField] GameObject _wallCollider;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _curveDestruct;

    [SerializeField] float _speedScreenShake = 2;
    [SerializeField] float _strengthScreenShake = 0.2f;

    private MaterialPropertyBlock _mpb;
    private IEnumerator _coroutine;

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




}
