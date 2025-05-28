using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class S_GhostRecorder : MonoBehaviour{
    public S_playerManagerStates S_playerManagerStates;
    public Animator animator;
    public float recordInterval = 0.01f;
    private float recordTimer = 0f;

    public List<S_GhostFrame> RecordedFrames { get; private set; } = new List<S_GhostFrame>();

    public void RecordFrame(float currentTime)
    {
        recordTimer += Time.unscaledDeltaTime;

        if (recordTimer >= recordInterval)
        {
            var animParams = new Dictionary<string, object>();
            foreach (var param in animator.parameters)
            {
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Float:
                        animParams[param.name] = animator.GetFloat(param.name);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animParams[param.name] = animator.GetInteger(param.name);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animParams[param.name] = animator.GetBool(param.name);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        break;
                }
            }
            bool isBall = S_playerManagerStates.IsDashing;
            bool facingR = S_playerManagerStates.FacingRight;
            RecordedFrames.Add(new S_GhostFrame(transform.position, currentTime, animParams, isBall, facingR));
            recordTimer = 0f;
        }
    }

    public void ResetRecording(){
        RecordedFrames.Clear();
        recordTimer = 0f;
    }
}