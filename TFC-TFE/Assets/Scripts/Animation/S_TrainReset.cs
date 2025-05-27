using System;
using UnityEngine;
using UnityEngine.Splines;

public class S_TrainReset : MonoBehaviour
{
    public static Action OnResetSpline;
    [SerializeField] private SplineAnimate _splineAnimate;
    [SerializeField] private SoundSystem _SFX_TchouTchou;
    [SerializeField] private GameObject _train;
    private void OnEnable()
    {
        OnResetSpline += ResetTrain;
        _SFX_TchouTchou.Play(_train.transform.position);
    }

    private void OnDisable()
    {
        OnResetSpline -= ResetTrain;
    }
    private void ResetTrain(){
        if (_splineAnimate is not null){
            _splineAnimate.ElapsedTime = 0f; 
        }
    }
}

