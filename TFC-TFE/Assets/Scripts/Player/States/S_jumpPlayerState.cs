using UnityEngine;

public class S_JumpPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        if (Player.IsGrounded && Player.CanJump)
        {
            // Calcule une force de saut dynamique selon la vitesse actuelle
            float speedRatio = Mathf.InverseLerp(Player.Settings.minMoveSpeed, Player.Settings.maxMoveSpeed, Mathf.Abs(Player.Rigidbody.velocity.z));
            float jumpForce = Mathf.Lerp(Player.Settings.minJumpForce, Player.Settings.maxJumpForce, speedRatio);

            // Applique une impulsion vers le haut
            Player.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Player.IsGrounded = false;
            Player.CanJump = false;

            Player.SwitchState(Player.AirState);
        }
        else if (Player.CanWallJump || Player.WallJumpTimer > 0)
        {
            // Reset vitesse avant de sauter
            Player.Rigidbody.velocity = Vector3.zero;

            Vector2 dir = Player.Settings.directionImpulsion;

            // Applique une impulsion dans une direction personnalisée (souvent diagonale)
            Player.Rigidbody.AddForce(new Vector3(dir.x, dir.y, 0).normalized * Player.Settings.wallJumpForce, ForceMode.Impulse);

            Player.CanWallJump = false;
            Player.WallJumpTimer = 0f;

            Player.SwitchState(Player.AirState);
        }
    }

    public override void OnEnable(S_playerStates Player) { }
    public override void OnDisable(S_playerStates Player) { }
    public override void UpdateState(S_playerStates Player) { }
}
