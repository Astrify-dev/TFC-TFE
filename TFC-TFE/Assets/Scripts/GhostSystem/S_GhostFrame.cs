using System.Collections.Generic;
using UnityEngine;

public class S_GhostFrame{
    public Vector3 position;
    public float time;
    public Dictionary<string, object> animatorParameters;
    public bool isBallMode;
    public bool facingRight;

    public S_GhostFrame(Vector3 pos, float t, Dictionary<string, object> animParams, bool ballMode, bool fr)
    {
        position = pos;
        time = t;
        animatorParameters = animParams;
        isBallMode = ballMode;
        facingRight = fr;
    }
}