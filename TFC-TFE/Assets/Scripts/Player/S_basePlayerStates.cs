using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class S_basePlayerStates 
{
    public abstract void EnterState(S_playerManagerStates Player);

    public abstract void UpdateState(S_playerManagerStates Player);

    public abstract void OnEnable(S_playerManagerStates Player);

    public abstract void OnDisable(S_playerManagerStates Player);
}
