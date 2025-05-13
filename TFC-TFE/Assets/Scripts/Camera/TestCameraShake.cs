using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraShake : MonoBehaviour
{
    [SerializeField] private S_cameraShake _shake;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _strength = 2;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            _shake.Shake(_speed, _strength);
        }
    }
}
