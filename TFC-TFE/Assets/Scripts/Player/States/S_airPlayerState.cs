using UnityEngine;

public class S_airPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        Player.IsGrounded = false;
        Player.CanJump = false;
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player) { }

    public override void UpdateState(S_playerStates Player)
    {
        if (Player.CheckGrounded())
        {
            Player.SwitchState(Player.GroundState);
            return;
        }

        // ❗ SUPPRIME la condition "MoveInput.x != 0" pour détecter le mur même sans input
        if (Player.CheckWall())
        {
            Player.SwitchState(Player.SlideWallState);
            return;
        }

        // Contrôle horizontal en l'air
        float targetSpeed = Player.MoveInput.x * Player.Settings.maxMoveSpeed;
        Vector3 velocity = Player.Rigidbody.velocity;
        velocity.z = Mathf.MoveTowards(velocity.z, targetSpeed, Player.Settings.accelerationRate * Time.deltaTime);
        Player.Rigidbody.velocity = velocity;

        Player.HandleFlip(Player.MoveInput.x);

        // ❗ Blocage du dive quand collé au mur
        if (Player.MoveInput.y < 0 && !Player.IsWallSliding)
        {
            Vector3 diveForce = new Vector3(0, Player.MoveInput.y, 0) * Player.Settings.dive;
            Player.Rigidbody.AddForce(diveForce, ForceMode.Acceleration);
        }
    }
}
