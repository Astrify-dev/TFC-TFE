using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_hairFollow : MonoBehaviour
{
    [SerializeField] GameObject _target1;
    [SerializeField] GameObject _target2;
    [SerializeField] GameObject _target3;

    [SerializeField] float _speed;

    private Vector3 _posTarget1;
    private Vector3 _posTarget2;
    private Vector3 _posTarget3;

    private void Start()
    {
        _posTarget1 = _target1.transform.localPosition;
        _posTarget2 = _target2.transform.localPosition;
        _posTarget2 = _target2.transform.localPosition;
    }

    private void Update()
    {
        _target1.transform.position = Vector3.MoveTowards(_target1.transform.position,transform.position - _posTarget1, _speed * Time.deltaTime);
        _target2.transform.position = Vector3.MoveTowards(_target2.transform.position, _target1.transform.localPosition - _posTarget2, _speed * Time.deltaTime);
        _target3.transform.position = Vector3.MoveTowards(_target3.transform.position, _target2.transform.localPosition - _posTarget3, _speed * Time.deltaTime);
    }
}
