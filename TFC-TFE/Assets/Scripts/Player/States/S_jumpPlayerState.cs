using UnityEngine;

public class S_JumpPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        if (Player.IsGrounded && Player.CanJump)
        {
            float speedRatio = Mathf.InverseLerp(Player.Settings.minMoveSpeed, Player.Settings.maxMoveSpeed, Mathf.Abs(Player.Rigidbody.velocity.z));
            float jumpForce = Mathf.Lerp(Player.Settings.minJumpForce, Player.Settings.maxJumpForce, speedRatio);

            Player.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Player.IsGrounded = false;
            Player.CanJump = false;

            Player.SwitchState(Player.AirState);
        }
        else if (Player.CanWallJump || Player.WallJumpTimer > 0)
        {
            Player.Rigidbody.velocity = Vector3.zero;

            //Utilise directionImpulsion mais appliquée sur Y (vertical) et Z (horizontal)
            int direction = Player.FacingRight ? -1 : 1;
            Vector2 impulse = new Vector2(direction * Player.Settings.directionImpulsion.x, Player.Settings.directionImpulsion.y);

            Vector3 wallJumpForce = new Vector3(0, impulse.y, impulse.x) * Player.Settings.wallJumpForce;
            Player.Rigidbody.AddForce(wallJumpForce, ForceMode.Impulse);

            Player.HandleFlip(direction);

            Player.CanWallJump = false;
            Player.WallJumpTimer = 0f;

            Player.SwitchState(Player.AirState);
        }
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player) { }

    public override void UpdateState(S_playerStates Player) { }
}
