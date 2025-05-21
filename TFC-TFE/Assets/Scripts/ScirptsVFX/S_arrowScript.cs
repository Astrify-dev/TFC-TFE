using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_arrowScript : MonoBehaviour
{
    [SerializeField] GameObject _arrowMesh;
    [SerializeField] AnimationCurve _arrowSpawnCurve;
    [SerializeField] float _arrowSpawnSpeed;
    [SerializeField] Vector2 _arrowSize;

    private bool _isActive = false;
    private float _time = 0;

    private void Start()
    {
        ArrowReset();
    }

    public void ArrowSpawn(float Angle)
    {
        if (!_isActive) Initialize();

        transform.eulerAngles = new Vector3(Angle, 0,0);

        Animation();
    }

    public void ArrowReset()
    {
        _isActive = false ;
        _arrowMesh.transform.localScale = new Vector3 (0, 0,0);
        _time = 0;
    }
    


    private void Initialize()
    {
        _isActive = true;
        _arrowMesh.transform.localScale = new Vector3(0, _arrowSize.y, 0);
        _time = 0;
    }

    private void Animation()
    {
        _time += Time.unscaledDeltaTime * _arrowSpawnSpeed;
        float size = _arrowSpawnCurve.Evaluate(_time) * _arrowSize.x;
        _arrowMesh.transform.localScale = new Vector3(size, _arrowMesh.transform.localScale.y, 0);
    }
}
