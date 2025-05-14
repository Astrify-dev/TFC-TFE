using UnityEngine;

public class S_groundPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        Player.IsGrounded = true;
        Player.CanJump = true;
        Player.CanDash = true;
        Player.CanAirDash = true;
        Player.HasAirDashed = false;
        Player.IsWallSliding = false;
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player) { }

    public override void UpdateState(S_playerStates Player)
    {
        if (!Player.CheckGrounded())
        {
            Player.SwitchState(Player.AirState);
            return;
        }

        float moveInput = Player.MoveInput.x;
        float targetSpeed = moveInput * Player.Settings.maxMoveSpeed;
        Vector3 velocity = Player.Rigidbody.velocity;

        // Modifie uniquement la vitesse horizontale (z)
        velocity.z = Mathf.MoveTowards(velocity.z, targetSpeed, Player.Settings.accelerationRate * Time.deltaTime);
        Player.Rigidbody.velocity = velocity;

        Player.HandleFlip(moveInput);
    }
}
