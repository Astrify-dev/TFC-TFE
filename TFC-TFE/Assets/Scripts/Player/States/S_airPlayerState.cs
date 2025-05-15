using UnityEngine;

public class S_airPlayerState : S_basePlayerStates
{
    private bool _justEntered = false;

    public override void EnterState(S_playerStates Player)
    {
        Player.IsGrounded = false;
        Player.CanJump = false;
        _justEntered = true;
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player) { }

    public override void UpdateState(S_playerStates Player)
    {
        if (Player.MovementLockTimer > 0f)
        {
            Player.MovementLockTimer -= Time.deltaTime;
            return; // Ne rien faire tant que le saut est verrouillé
        }

        if (Player.IsDashing){
            return;
        }

        if (_justEntered)
        {
            _justEntered = false;
            return;
        }

        if (Player.CheckGrounded())
        {
            Player.SwitchState(Player.GroundState);
            return;
        }

        if (Player.CheckWall())
        {
            Player.SwitchState(Player.SlideWallState);
            return;
        }

        // ❗ Appliquer air control uniquement si non verrouillé
        if (Player.AirControlLockTimer <= 0f)
        {
            float targetSpeed = Player.MoveInput.x * Player.Settings.airMaxMoveSpeed;
            Vector3 velocity = Player.Rigidbody.velocity;
            velocity.z = Mathf.MoveTowards(velocity.z, targetSpeed, Player.Settings.airAccelerationRate * Time.deltaTime);
            Player.Rigidbody.velocity = velocity;
        }


        Player.HandleFlip(Player.MoveInput.x);

        // Dive ou chute
        if (Player.MoveInput.y < 0 && !Player.IsWallSliding)
        {
            Vector3 diveForce = new Vector3(0, Player.MoveInput.y, 0) * Player.Settings.dive;
            Player.Rigidbody.AddForce(diveForce, ForceMode.Acceleration);
        }
        else
        {
            Vector3 vel = Player.Rigidbody.velocity;
            vel.y = Mathf.Max(vel.y, -Player.Settings.fallSpeed); // limite chute naturelle
            Player.Rigidbody.velocity = vel;
        }
    }
}
