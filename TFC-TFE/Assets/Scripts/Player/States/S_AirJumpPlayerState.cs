using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_AirJumpPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerManagerStates Player)
    {
        int direction = Player.DashDirection.z < 0 ? -1 : 1;

        Vector2 impulse = new Vector2(direction * Player.MovementSettings.dashJumpDirection.x, Player.MovementSettings.dashJumpDirection.y);
        Vector3 wallJumpForce = new Vector3(0, impulse.y, impulse.x) * Player.MovementSettings.dashJumpForce;

        Player.Rigidbody.AddForce(wallJumpForce, ForceMode.Impulse);

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
