using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CheckpointAnim : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] Animator checkpointAnimator;
    

    void Update()
    {
        float rotationSpeed = speed * Time.deltaTime;

        if (checkpointAnimator.GetBool("IsActivated"))
        {
            transform.Rotate(0f, 0f, rotationSpeed);
        }
    }
}
