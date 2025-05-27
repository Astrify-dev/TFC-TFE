using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class S_switchColorHair : MonoBehaviour
{
    [Header("ColorSwitch")]
    [SerializeField] AnimationCurve _colorSwitchCurve;
    [SerializeField] float _speedSwitchColor;
    [SerializeField] Color _colorEnable;
    [SerializeField] Color _colorDisable;
    [SerializeField] VisualEffect _effectHair;

    private IEnumerator _switchColor;

    public void ColorSwitch(bool Highlight)
    {
        if (_switchColor is not null)
            StopCoroutine(_switchColor);

        _switchColor = ColorSwitchAnim(Highlight);
        StartCoroutine(_switchColor);
    }

    IEnumerator ColorSwitchAnim(bool Enable)
    {
        float Timer = 0;
        Color ValueUpdate;
        Color LastColor = Enable ? _colorDisable : _colorEnable;
        Color NewColor = Enable ? _colorEnable : _colorDisable;

        while (Timer < 1)
        {
            Timer += Time.deltaTime * _speedSwitchColor;

            ValueUpdate = Color.Lerp(LastColor, NewColor, _colorSwitchCurve.Evaluate(Timer));

            _effectHair.SetVector4("BaseColor", ValueUpdate);

            yield return null;
        }

        _effectHair.SetVector4("BaseColor", Enable ? _colorEnable : _colorDisable);


    }
}
