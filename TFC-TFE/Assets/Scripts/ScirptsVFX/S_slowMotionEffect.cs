using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_slowMotionEffect : MonoBehaviour
{
    [SerializeField] AnimationCurve _enableEffectCurve;
    [SerializeField] Material _material;

    private float _valueMaterial;
    private IEnumerator _couroutineEffectEnable;

    private void Start()
    {
        _material.SetFloat("_EnableSlowMotion",0);
    }

    public void StartSlowMotion(bool Enable,float Speed)
    {
        if (_couroutineEffectEnable is not null)
            StopCoroutine(_couroutineEffectEnable);


        _couroutineEffectEnable = AnimationEffectSlowMotion(Enable, Speed);
        StartCoroutine(_couroutineEffectEnable);

    }

    IEnumerator AnimationEffectSlowMotion(bool Enable, float Speed)
    {
        float Timer = 0;
        _valueMaterial = Enable ? 0 : _valueMaterial;

        while(Timer < 1)
        {
            Timer += Time.unscaledDeltaTime * Speed;
            float Value = _enableEffectCurve.Evaluate(Enable ? Timer: 1-Timer);
            _valueMaterial = Enable ? Value : Mathf.Min(Value, _valueMaterial);

            _material.SetFloat("_EnableSlowMotion", _valueMaterial);

            yield return null;
        }

        _material.SetFloat("_EnableSlowMotion", Enable ? 1 : 0);
    }

}
