using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_vibrationGamePad : MonoBehaviour
{
    [SerializeField] AnimationCurve _progressCurve;

    IEnumerator _vibrationOn;

    public void StartVibration(float Duration, float Speed, float power)
    {
        Gamepad.current.SetMotorSpeeds(0f, 0f);

        if (_vibrationOn is not null)
            StopCoroutine(_vibrationOn);

        _vibrationOn = VibrationGamePad(Duration, Speed, power);
        StartCoroutine(_vibrationOn);
    }

    IEnumerator VibrationGamePad(float Duration, float Speed, float power)
    {
        float Timer = 0;
        float Value = 0;

        while (Timer < 0)
        {
            Timer += Time.unscaledDeltaTime * Duration;

            Value = _progressCurve.Evaluate(Timer);

            Debug.Log(Value);

            Gamepad.current.SetMotorSpeeds(Value * Speed, Value * power);

            yield return null;

        }

        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

}
