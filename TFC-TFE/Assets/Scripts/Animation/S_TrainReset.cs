using System;
using UnityEngine;
using UnityEngine.Splines;

public class S_TrainReset : MonoBehaviour
{
    public static Action OnResetSpline;
    [SerializeField] private SplineAnimate _splineAnimate;
    private void OnEnable()
    {
        OnResetSpline += ResetTrain;
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

