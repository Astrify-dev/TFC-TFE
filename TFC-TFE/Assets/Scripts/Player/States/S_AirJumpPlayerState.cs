using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_AirJumpPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerManagerStates Player)
    {
        Player.SwitchState(Player.AirState);
    }

    public override void OnDisable(S_playerManagerStates Player)
    {
        
    }

    public override void OnEnable(S_playerManagerStates Player)
    {
        
    }

    public override void UpdateState(S_playerManagerStates Player)
    {
        
    }
}
