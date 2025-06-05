using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SlowMotion : MonoBehaviour{
    private Coroutine _slowMotionCoroutine;
    private Coroutine _slowMotionAnimEnableCoroutine;
    public void StartSlowMotion(float intensity, float duration, System.Action onSlowMotionEnd = null)
    {
        if (_slowMotionCoroutine != null)
        {
            StopCoroutine(_slowMotionCoroutine);
        }
        _slowMotionCoroutine = StartCoroutine(HandleSlowMotion(intensity, duration, onSlowMotionEnd));
    }

    public void StopSlowMotion()
    {
        if (_slowMotionCoroutine != null)
        {
            StopCoroutine(_slowMotionCoroutine);
            _slowMotionCoroutine = null;
        }
        if(_slowMotionAnimEnableCoroutine != null)
        {
            StopCoroutine(_slowMotionAnimEnableCoroutine);
            _slowMotionAnimEnableCoroutine = null;
        }
        ResetTimeScale();
    }
    private IEnumerator HandleSlowMotion(float intensity, float duration, System.Action onSlowMotionEnd)
    {
        Time.timeScale = intensity;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        ResetTimeScale();
        onSlowMotionEnd?.Invoke();
    }
    public void StartSlowMotionAnimEnable(float Speed, float Intensity, AnimationCurve Curve)
    {
        if( _slowMotionAnimEnableCoroutine is not null)
            StopCoroutine(_slowMotionAnimEnableCoroutine);

        _slowMotionAnimEnableCoroutine = StartCoroutine(SlowMotionEnableAnim(Speed, Intensity, Curve));
    }
    public void StopSlowMotionAnimEnable()
    {
        if (_slowMotionAnimEnableCoroutine is not null)
            StopCoroutine(_slowMotionAnimEnableCoroutine);
    }

    IEnumerator SlowMotionEnableAnim(float Speed, float Intensity, AnimationCurve Curve)
    {
        float Timer = 0;
        ResetTimeScale();

        while (Timer < 1)
        {
            float CurveValue = Curve.Evaluate(Timer);
            Time.timeScale = Mathf.Lerp(1f, Intensity, CurveValue);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            Timer += Time.unscaledDeltaTime * Speed;
            Debug.Log("TimeScale: "+ Time.timeScale);
            yield return null;
        }

        Time.timeScale = Intensity;
        Time.fixedDeltaTime = Intensity * 0.02f;
    }

    private void ResetTimeScale()
    {
        Debug.Log("ResetTimeScale");
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
