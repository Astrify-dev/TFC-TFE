using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class S_hairFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] GameObject _target1;
    [SerializeField] GameObject _target2;
    [SerializeField] GameObject _target3;
    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _positionPlayer;
    

    [Header("Speed")]
    [SerializeField] float _speed1;
    [SerializeField] float _speed2;
    [SerializeField] float _speed3;
    [SerializeField] float _speedShort = 1.5f;

    [Header("turbulance")]
    [SerializeField] float _strengthRandom = 1;
    [SerializeField] float _maxDistanceRandom = 1;

    [Header("Physic")]
    [SerializeField] float _maxVelocity = 1;
    [SerializeField] float _velocityLose = 1.5f;

    [Space]
    [SerializeField] bool _drawGizmos;

    private Vector3 _pos1;
    private Vector3 _pos2;
    private Vector3 _pos3;

    private Vector3 _posTarget1;
    private Vector3 _posTarget2;
    private Vector3 _posTarget3;

    private Vector3 _vectorTarget1;
    private Vector3 _vectorTarget2;
    private Vector3 _vectorTarget3;

    private Vector3 _smallPosTarget1;
    private Vector3 _smallPosTarget2;
    private Vector3 _smallPosTarget3;

    bool _short = false;

    private void Awake()
    {
        _pos1 = _target1.transform.position - _target2.transform.position;
        _pos2 = _target2.transform.position - _target3.transform.position;
        _pos3 = _target3.transform.position - transform.position;

        _smallPosTarget1 = _pos1 / 100;
        _smallPosTarget2 = _pos2 / 100;
        _smallPosTarget3 = _pos3 / 100;
        SetupPos();

    }
    private void OnEnable()
    {
        ResetHair();
    }

    private void Update()
    {
        transform.position = _player.transform.position + _positionPlayer;
    }

    public void SetupPos()
    {
        _posTarget1 = _pos1;
        _posTarget2 = _pos2;
        _posTarget3 = _pos3;

        _short = false;
    }

    public void SetUpSmallPos()
    {
        _posTarget1 = _smallPosTarget1;
        _posTarget2 = _smallPosTarget2;
        _posTarget3 = _smallPosTarget3;

        _short = true;
    }

    private void FixedUpdate()
    {
        float speedShort = _short ? _speedShort : 1;


        Vector3 posTargetRand = Turbulance(_posTarget3);

        _vectorTarget3 = DirectionTarget(transform.position + posTargetRand, _target3.transform, _speed3 * speedShort, _vectorTarget3);

        posTargetRand = Turbulance(_posTarget2);

        _vectorTarget2 = DirectionTarget(_target3.transform.position + posTargetRand, _target2.transform, _speed2 * speedShort, _vectorTarget2);

        posTargetRand = Turbulance(_posTarget1);

        _vectorTarget1 = DirectionTarget(_target2.transform.position + posTargetRand, _target1.transform, _speed1 * speedShort, _vectorTarget1);

    }

    private Vector3 Turbulance(Vector3 pos)
    {
        Vector3 posRand = Random.onUnitSphere * _strengthRandom;
        posRand = Vector3.ClampMagnitude(posRand, _maxDistanceRandom);
        return pos + posRand;
    }

    private Vector3 DirectionTarget(Vector3 target,Transform currentPos,float speed,Vector3 vec)
    {
        Vector3 dir;

        dir = target - currentPos.transform.position;
        dir *= (speed * Time.deltaTime);

        vec += dir;

        vec = Vector3.ClampMagnitude(vec, _maxVelocity);

        currentPos.transform.position += vec;
        return vec/ _velocityLose;
    }

    public void FlipHair(bool Right)
    {
        Debug.Log("HairRight:" + Right);

        _positionPlayer.z = -Mathf.Abs(_positionPlayer.z);
        _posTarget1.z = -Mathf.Abs(_posTarget1.z);
        _posTarget2.z = Mathf.Abs(_posTarget2.z);
        _posTarget3.z = Mathf.Abs(_posTarget3.z);

        if (Right)
        {
            _positionPlayer.z *= -1;
            _posTarget1.z *= -1;
            _posTarget2.z *= -1;
            _posTarget3.z *= -1;
        }
       
        
    }

    public void ResetHair()
    {
        transform.position = _player.transform.position + _positionPlayer;
        _target3.transform.position = transform.position;
        _target2.transform.position = transform.position;
        _target1.transform.position = transform.position;

        FlipHair(S_controllerPlayer.Instance.PlayerManagerStates.FacingRight);
    }

    private void OnDrawGizmos()
    {
        if(!_drawGizmos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_target1.transform.position, _strengthRandom);
        Gizmos.DrawWireSphere(_target2.transform.position, _strengthRandom);
        Gizmos.DrawWireSphere(_target3.transform.position, _strengthRandom);
    }
}
