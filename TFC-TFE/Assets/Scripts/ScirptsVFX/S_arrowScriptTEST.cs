using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_arrowScriptTEST : MonoBehaviour
{
    [SerializeField] S_arrowScript _arrowScript;
    [SerializeField] Vector3 _direction;

    private bool _reset = false;

    private void Start()
    {
        _arrowScript.ArrowReset();
    }

    private void Update()
    {
        if (_direction == Vector3.zero)
        {
            if (_reset) return;
            _reset = true;
            _arrowScript.ArrowReset();
            return;
        }
        _reset = false;
        _arrowScript.ArrowSpawn(_direction);
    }
}
