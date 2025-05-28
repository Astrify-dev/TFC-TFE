using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class S_deathAlongueSpline : MonoBehaviour
{

    [SerializeField] SplineContainer _splineFollow;
    [SerializeField] GameObject _deathObject;

    [SerializeField] float _maxSpeed;
    [SerializeField] AnimationCurve _speedCurve;

    [SerializeField] Material _materialCorruption;

    [Header("UpdateDeadZone")]
    [SerializeField] int _updateCount;


    [Header("GizmosHelps")]
    [SerializeField] bool _disableGiemos;
    [SerializeField] private int _gizmosCount = 100;
    [SerializeField] private float _gizmosSize = 1;

    public static Action RestDeadZone;

    public static event Action<float> UpdateCollider;

    private int _countUpdateCollider;
    private float _splineDistance;
    private float _currentDistancePercentage;

    private void Awake()
    {
        Vector3 NewPosition = _splineFollow.EvaluatePosition(0);
        _deathObject.transform.position = NewPosition;

    }

    private void Start()
    {
        _materialCorruption.SetInt("_EnableEffect", 1);
        _splineDistance = _splineFollow.CalculateLength();
        resetDeadZone();
    }

    private void OnEnable()
    {
        RestDeadZone += resetDeadZone;
    }

    private void OnDisable()
    {
        RestDeadZone -= resetDeadZone;
        _materialCorruption.SetInt("_EnableEffect", 0);
    }

    private void Update()
    {
        float currentSpeed = _speedCurve.Evaluate(_currentDistancePercentage) * _maxSpeed;
        _currentDistancePercentage += (currentSpeed * Time.deltaTime) / _splineDistance;

        if (_currentDistancePercentage > 1)
            EndGame();

        if (_currentDistancePercentage * _updateCount > _countUpdateCollider)
        {
            _countUpdateCollider += 1;
            UpdateCollider?.Invoke(_currentDistancePercentage);
        }

        Vector3 NewPosition = _splineFollow.EvaluatePosition(_currentDistancePercentage);

        _materialCorruption.SetVector("_Coord", NewPosition);

        _deathObject.transform.position = NewPosition;
    }

    private void EndGame()
    {
        S_playerManagerStates ManagerStates = S_controllerPlayer.Instance.PlayerManagerStates;

        ManagerStates.SwitchState(ManagerStates.DeadState);

        resetDeadZone();
        S_TimerSpeedrun.OnPlayerDeath?.Invoke();
        S_CanvasEnd.OnPlayerDeath?.Invoke();
    }

    private void resetDeadZone()
    {
        _currentDistancePercentage = 0;
        Vector3 NewPosition = _splineFollow.EvaluatePosition(0);
        Debug.Log(NewPosition);
        _deathObject.transform.position = NewPosition;
    }

    private void OnDrawGizmos()
    {
        if(_disableGiemos || _splineFollow is null) return;

        for (int i = 0; i < _gizmosCount; i++)
        {
            float Value = (float)i/_gizmosCount;
            Vector3 GizmosPos = _splineFollow.EvaluatePosition(Value);

            Gizmos.color = Color.Lerp(Color.green, Color.red, _speedCurve.Evaluate(Value));
            Gizmos.DrawWireSphere(GizmosPos, _gizmosSize);

        }

        for (int i = 0; i < _updateCount; i++)
        {
            float Value = (float)i / _updateCount;
            Vector3 GizmosPos = _splineFollow.EvaluatePosition(Value);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GizmosPos,new Vector3(_gizmosSize/5, _gizmosSize*4, _gizmosSize/2));

        }


    }

}
