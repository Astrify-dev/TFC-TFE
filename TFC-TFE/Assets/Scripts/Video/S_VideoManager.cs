using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class S_VideoManager : MonoBehaviour
{
    [SerializeField] VolumeProfile _volumeProfile;
    private LiftGammaGain _liftGammaGain;
    private ColorAdjustments _colorAdjustments;
    [MinMaxSlider(-100, 100), SerializeField] Vector2 _saturationMaxMin;
    [MinMaxSlider(-2f, 2f), SerializeField] Vector2 _luminosityMaxMin;

    private void Start()
    {
        if (_volumeProfile.TryGet<LiftGammaGain>(out LiftGammaGain Gamma))
            _liftGammaGain = Gamma;

        if (_volumeProfile.TryGet<ColorAdjustments>(out ColorAdjustments ColorAdjustments))
            _colorAdjustments = ColorAdjustments;

        if (!PlayerPrefs.HasKey("Saturation")) SetStartValue(0.5f);
        else SetValue();
    }

    private void SetStartValue(float value)
    {
        float ClampValue = Mathf.Lerp(_saturationMaxMin.x, _saturationMaxMin.y, value);
        _colorAdjustments.saturation.Override(ClampValue);

        ClampValue = Mathf.Lerp(_luminosityMaxMin.x, _luminosityMaxMin.y, value);
        _liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, ClampValue));

        PlayerPrefs.SetFloat("Saturation", value);
        PlayerPrefs.SetFloat("Gamma", value);

        PlayerPrefs.Save();
    }

    private void SetValue()
    {
        float ClampValue = Mathf.Lerp(_saturationMaxMin.x, _saturationMaxMin.y, PlayerPrefs.GetFloat("Saturation"));
        _colorAdjustments.saturation.Override(ClampValue);

        ClampValue = Mathf.Lerp(_luminosityMaxMin.x, _luminosityMaxMin.y, PlayerPrefs.GetFloat("Gamma"));
        _liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, ClampValue));
    }
}
