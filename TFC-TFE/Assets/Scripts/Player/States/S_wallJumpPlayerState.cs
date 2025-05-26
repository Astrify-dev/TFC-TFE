using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class S_wallJumpPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerManagerStates Player)
    {
        Player.SFX_WallJump.Play();
        int direction = Player.FacingRight ? -1 : 1;

        Player.StartReloadWallSlide();

        Vector2 impulse = new Vector2(direction * Player.MovementSettings.directionImpulsion.x, Player.MovementSettings.directionImpulsion.y);
        Vector3 wallJumpForce = new Vector3(0, impulse.y, impulse.x) * Player.MovementSettings.wallJumpForce;

        Player.Rigidbody.AddForce(wallJumpForce, ForceMode.Impulse);
        Player.HandleFlip(direction);

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
