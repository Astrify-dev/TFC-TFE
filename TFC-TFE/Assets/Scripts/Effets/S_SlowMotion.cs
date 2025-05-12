using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SlowMotion : MonoBehaviour{
    private Coroutine _slowMotionCoroutine;

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

    private void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
