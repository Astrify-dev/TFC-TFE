using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_cameraShake : MonoBehaviour
{
    [SerializeField] private AnimationCurve _shakeCurve;

    IEnumerator _shakeCamera;


    public void Shake(float Speed,float Strength)
    {
        if(_shakeCamera is not null)
        {
            StopCoroutine(_shakeCamera);
        }
        _shakeCamera = Shaking(Speed, Strength);
        StartCoroutine(_shakeCamera);
    }

    private IEnumerator Shaking(float Speed, float Strength)
    {
        float timer = 0;
        Vector3 CamPosition;

        gameObject.transform.localPosition = Vector3.zero;

        while (1 > timer)
        {
            timer += Speed * Time.deltaTime;

            CamPosition = Random.insideUnitSphere * Strength * _shakeCurve.Evaluate(timer);
            gameObject.transform.localPosition = CamPosition;
            yield return null;
        }

        gameObject.transform.localPosition = Vector3.zero;
    }
}
