using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_controllerPlayer : MonoBehaviour{
    public static S_controllerPlayer Instance { get; private set; }

    [Header("Références des scripts liés au joueur")]
    public S_inputPlayer inputPlayer;
    public S_SlowMotion slowMotionHandler;

    [field: SerializeField] public S_slowMotionEffect SlowMotionEffect { get; private set;}
    [field: SerializeField] public S_arrowScript ArrowEffect { get; private set; }
    [field: SerializeField] public S_cameraShake CameraShake { get; private set; }

    private void Awake(){
        if (inputPlayer is null){
            inputPlayer = GetComponent<S_inputPlayer>();
        }

        if (Instance is not null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (inputPlayer is null){
            inputPlayer = GetComponent<S_inputPlayer>();
        }
        if (slowMotionHandler is null){
            slowMotionHandler = GetComponent<S_SlowMotion>();
        }

        if(SlowMotionEffect is null)
        {
            SlowMotionEffect = GetComponent<S_slowMotionEffect>();
        }

        if(ArrowEffect is null)
        {
            ArrowEffect = GetComponent<S_arrowScript>();
        }

        if (CameraShake is null)
        {
            Debug.LogError("Not CameraShake Scripts");
        }

    }

}
