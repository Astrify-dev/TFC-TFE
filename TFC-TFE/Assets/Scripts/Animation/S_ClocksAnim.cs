using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ClocksAnim : MonoBehaviour{
    public Transform minuteHand;
    public Transform hourHand;
    public float speedMultiplier = 1f;

    void Update(){
        float minuteRotation = -6f * Time.deltaTime * speedMultiplier;
        float hourRotation = -0.5f * Time.deltaTime * speedMultiplier;

        if (minuteHand is not null){
            minuteHand.Rotate(minuteRotation, 0f, 0f);
        }

        if (hourHand is not null){
            hourHand.Rotate(hourRotation, 0f, 0f);
        }
    }
}