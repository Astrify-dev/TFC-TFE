using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_hairFollow : MonoBehaviour
{
    [SerializeField] GameObject _target1;
    [SerializeField] GameObject _target2;
    [SerializeField] GameObject _target3;

    [SerializeField] float _speed1;
    [SerializeField] float _speed2;
    [SerializeField] float _speed3;
    [SerializeField] float _strengthRandom = 1;
    [SerializeField] float _maxVelocity = 1;

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
        //Vector3 pos1 = _posTarget1 + Random.insideUnitSphere * _strengthRandom;
        //Vector3 pos2 = _posTarget2 + Random.insideUnitSphere * _strengthRandom;
        //Vector3 pos3 = _posTarget3 + Random.insideUnitSphere * _strengthRandom;

        //_target1.transform.position = Vector3.MoveTowards(_target1.transform.position, _target2.transform.position + pos1, _speed * Time.deltaTime);
        //_target2.transform.position = Vector3.MoveTowards(_target2.transform.position, _target3.transform.position + pos2, _speed * Time.deltaTime);
        //_target3.transform.position = Vector3.MoveTowards(_target3.transform.position, transform.position + pos3, _speed * Time.deltaTime);

        _vectorTarget3 = DirectionTarget(transform.position + _posTarget3, _target3.transform, _speed3, _vectorTarget3);

        _vectorTarget2 = DirectionTarget(_target3.transform.position + _posTarget2, _target2.transform, _speed2, _vectorTarget2);

        _vectorTarget1 = DirectionTarget(_target2.transform.position + _posTarget1, _target1.transform, _speed1, _vectorTarget1);

    }

    private Vector3 DirectionTarget(Vector3 target,Transform currentPos,float speed,Vector3 vec)
    {
        Vector3 dir;

        dir = target - currentPos.transform.position;
        dir *= (speed * Time.deltaTime);

        vec += dir;

        vec = Vector3.ClampMagnitude(vec, _maxVelocity);

        currentPos.transform.position += vec;
        return vec;
    }
}
