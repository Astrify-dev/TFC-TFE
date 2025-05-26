using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GhostPlayer : MonoBehaviour{
    public GameObject visualObject;
    public GameObject meshChara;
    public Animator animator;
    public GameObject ballVFXObject;
    private List<S_GhostFrame> replayFrames;
    private int currentIndex = 0;

    public void StartReplay(List<S_GhostFrame> frames)
    {
        replayFrames = frames;
        currentIndex = 0;
    }

    public void UpdateGhost(float currentTime)
    {
        if (replayFrames == null || replayFrames.Count < 2)
            return;

        while (currentIndex < replayFrames.Count - 2 &&
               replayFrames[currentIndex + 1].time < currentTime)
        {
            currentIndex++;
        }

        S_GhostFrame from = replayFrames[currentIndex];
        S_GhostFrame to = replayFrames[currentIndex + 1];

        float t = Mathf.InverseLerp(from.time, to.time, currentTime);
        transform.position = Vector3.Lerp(from.position, to.position, t);

        foreach (var kvp in from.animatorParameters)
        {
            string param = kvp.Key;
            object value = kvp.Value;

            if (value is float f)
                animator.SetFloat(param, f);
            else if (value is int i)
                animator.SetInteger(param, i);
            else if (value is bool b)
                animator.SetBool(param, b);
        }
        if(ballVFXObject is not null){
            ballVFXObject.SetActive(from.isBallMode);
            meshChara.SetActive(!from.isBallMode);
        }
            

        if(visualObject is not null)
        {
            Vector3 rot = visualObject.transform.eulerAngles;
            rot.y = from.facingRight ? 0f : 180f;
            visualObject.transform.eulerAngles = rot;
        }
    }
}