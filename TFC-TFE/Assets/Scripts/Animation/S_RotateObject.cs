using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RotateObject : MonoBehaviour{
    public float rotationSpeedX = 0f;
    public float rotationSpeedY = 0f;
    public float rotationSpeedZ = 0f;

    void Update(){
        float rotationX = rotationSpeedX * Time.deltaTime;
        float rotationY = rotationSpeedY * Time.deltaTime;
        float rotationZ = rotationSpeedZ * Time.deltaTime;

        transform.Rotate(rotationX, rotationY, rotationZ);
    }
}