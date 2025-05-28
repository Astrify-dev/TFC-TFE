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

        if (Gamepad.current is null) return;
        Gamepad.current.SetMotorSpeeds(0f, 0f);

        if (_vibrationOn is not null)
            StopCoroutine(_vibrationOn);

        _vibrationOn = VibrationGamePad(Duration, Speed, power);
        StartCoroutine(_vibrationOn);
    }

    public void StopVibration()
    {
        if (_vibrationOn is not null)
            StopCoroutine(_vibrationOn);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    IEnumerator VibrationGamePad(float Duration, float Speed, float power)
    {
        float Timer = 0;
        float Value = 0;

        while (Timer < 1)
        {
            Timer += Time.unscaledDeltaTime * Duration;

            Value = _progressCurve.Evaluate(Timer);

            Gamepad.current.SetMotorSpeeds(Value * Speed, Value * power);

            yield return null;

        }

        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

}
