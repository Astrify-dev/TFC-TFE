using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_controllerPlayer : MonoBehaviour
{
    public static S_controllerPlayer Instance { get; private set; }

    [Header("R�f�rences des scripts li�s au joueur")]
    public S_inputPlayer inputPlayer;
    public S_SlowMotion slowMotionHandler;

    [field: SerializeField] public S_slowMotionEffect SlowMotionEffect { get; private set; }
    [field: SerializeField] public S_arrowScript ArrowEffect { get; private set; }

    [field: SerializeField] public Animator AnimatorPlayer { get; private set; }

    [field: SerializeField] public S_cameraShake CameraShake { get; private set; }

    [field: SerializeField] public S_playerManagerStates PlayerManagerStates { get; private set; }

    [field: SerializeField] public S_hairFollow HairFollow { get; private set; }

    [field: SerializeField] public S_switchColorHair ColorSwitchHair { get; private set; }
    [field: SerializeField] public S_particleDashManager ParticleDashManager { get; private set; }

    [field: SerializeField] public S_vibrationGamePad VibrationGamePad { get; private set; }

    private void Awake()
    {
        if (inputPlayer is null)
        {
            inputPlayer = GetComponent<S_inputPlayer>();
        }

        if (Instance is not null && Instance != this)
        {
            //Destroy(gameObject);
            //return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (inputPlayer is null)
        {
            inputPlayer = GetComponent<S_inputPlayer>();
        }
        if (slowMotionHandler is null)
        {
            slowMotionHandler = GetComponent<S_SlowMotion>();
        }

        if (SlowMotionEffect is null)
        {
            SlowMotionEffect = GetComponent<S_slowMotionEffect>();
        }

        if (ArrowEffect is null)
        {
            ArrowEffect = GetComponent<S_arrowScript>();
        }

        if (AnimatorPlayer is null)
        {
            AnimatorPlayer = GetComponent<Animator>();

        }

        if (CameraShake is null)
        {
            Debug.LogError("Not CameraShake Scripts");

        }

        if(PlayerManagerStates is null)
        {
            PlayerManagerStates = GetComponent<S_playerManagerStates>();
        }

        if (HairFollow is null)
        {
            Debug.LogError("Not HairFollow Scripts");

        }

        if (ParticleDashManager is null)
        {
            Debug.LogError("Not ParticleDashManager Scripts");
        }

        if(ColorSwitchHair is null)
        {
            Debug.LogError("Not ColorSwitchHair Scripts");

        }

        if(VibrationGamePad is null)
        {
            VibrationGamePad = GetComponent<S_vibrationGamePad>();
        }

    }
}
