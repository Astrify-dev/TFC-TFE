using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_hairFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] GameObject _playerTarget;
    [SerializeField] Vector3 _positionHair;
    [SerializeField] GameObject _target1;
    [SerializeField] GameObject _target2;
    [SerializeField] GameObject _target3;

    [Header("Speed")]
    [SerializeField] float _speed1;
    [SerializeField] float _speed2;
    [SerializeField] float _speed3;

    [Header("turbulance")]
    [SerializeField] float _strengthRandom = 1;
    [SerializeField] float _maxDistanceRandom = 1;

    [Header("Physic")]
    [SerializeField] float _maxVelocity = 1;
    [SerializeField] float _velocityLose = 1.5f;

    [Space]
    [SerializeField] bool _drawGizmos;

    private Vector3 _posTarget1;
    private Vector3 _posTarget2;
    private Vector3 _posTarget3;

    private Vector3 _vectorTarget1;
    private Vector3 _vectorTarget2;
    private Vector3 _vectorTarget3;

    private void Start()
    {
        _posTarget1 = _target1.transform.position - _target2.transform.position;
        _posTarget2 = _target2.transform.position - _target3.transform.position;
        _posTarget3 = _target3.transform.position - transform.position;
    }

    private void Update()
    {
        transform.position = _playerTarget.transform.position + _positionHair;
    }

    private void FixedUpdate()
    {

        Vector3 posTargetRand = Turbulance(_posTarget3);

        _vectorTarget3 = DirectionTarget(transform.position + posTargetRand, _target3.transform, _speed3, _vectorTarget3);

        posTargetRand = Turbulance(_posTarget2);

        _vectorTarget2 = DirectionTarget(_target3.transform.position + posTargetRand, _target2.transform, _speed2, _vectorTarget2);

        posTargetRand = Turbulance(_posTarget1);

        _vectorTarget1 = DirectionTarget(_target2.transform.position + posTargetRand, _target1.transform, _speed1, _vectorTarget1);

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

    private void OnDrawGizmos()
    {
        if(!_drawGizmos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_target1.transform.position, _strengthRandom);
        Gizmos.DrawWireSphere(_target2.transform.position, _strengthRandom);
        Gizmos.DrawWireSphere(_target3.transform.position, _strengthRandom);
    }
}
