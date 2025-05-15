using UnityEngine;

public class S_JumpPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        if (Player.IsGrounded && Player.CanJump)
        {
            Debug.Log("<color=green>[JUMP]</color> Ground jump triggered");
            Player.StartVariableJump();
        }
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player) { }

    public override void UpdateState(S_playerStates Player) { }
}
