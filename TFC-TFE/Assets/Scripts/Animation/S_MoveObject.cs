using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MoveObject : MonoBehaviour{
    public Vector3 floatAxis = Vector3.up;
    public float floatDistance = 1f;
    public float floatSpeed = 1f;

    private Vector3 startPosition;

    void Start(){
        startPosition = transform.position;
    }

    void Update(){
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatDistance;
        transform.position = startPosition + floatAxis.normalized * offset;
    }
}