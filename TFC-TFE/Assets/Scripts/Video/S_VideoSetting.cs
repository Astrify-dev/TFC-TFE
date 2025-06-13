using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Slider _saturation;
    [MinMaxSlider(-100,100), SerializeField] Vector2 _saturationMaxMin;
    [SerializeField] Slider _luminosity;
    [MinMaxSlider(-2f, 2f), SerializeField] Vector2 _luminosityMaxMin;


    [SerializeField] VolumeProfile _volumeProfile;
    private LiftGammaGain _liftGammaGain;
    private ColorAdjustments _colorAdjustments;

    private void Start()
    {
        _saturation.onValueChanged.AddListener(SetSaturation);
        _luminosity.onValueChanged.AddListener(SetGamma);

        if(_volumeProfile.TryGet<LiftGammaGain>(out LiftGammaGain Gamma))
            _liftGammaGain = Gamma;

        if(_volumeProfile.TryGet<ColorAdjustments>(out ColorAdjustments ColorAdjustments))
            _colorAdjustments = ColorAdjustments;

        SetValue();
    }

    private void SetValue()
    {
        _saturation.value = PlayerPrefs.GetFloat("Saturation") * _saturation.maxValue;
        _luminosity.value = PlayerPrefs.GetFloat("Gamma") * _saturation.maxValue;
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    private void SetSaturation(float value)
    {
        float SliderValue = value / _saturation.maxValue;

        float ClampValue = Mathf.Lerp(_saturationMaxMin.x,_saturationMaxMin.y, SliderValue);

        _colorAdjustments.saturation.Override(ClampValue);

        PlayerPrefs.SetFloat("Saturation", SliderValue);
    }

    private void SetGamma(float value)
    {
        float SliderValue = value/ _luminosity.maxValue;

        float ClampValue = Mathf.Lerp(_luminosityMaxMin.x, _luminosityMaxMin.y, SliderValue);

        _liftGammaGain.gamma.Override(new Vector4(1f,1f,1f, ClampValue));

        PlayerPrefs.SetFloat("Gamma", SliderValue);
    }
}
