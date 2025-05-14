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

        if (Player.CheckWall() && Player.MoveInput.x != 0)
        {
            Player.SwitchState(Player.SlideWallState);
            return;
        }

        // Mouvement horizontal en l'air (contrôle du joueur)
        float targetSpeed = Player.MoveInput.x * Player.Settings.maxMoveSpeed;
        Vector3 velocity = Player.Rigidbody.velocity;

        // Adoucit la transition de vitesse pour plus de fluidité
        velocity.z = Mathf.MoveTowards(velocity.z, targetSpeed, Player.Settings.accelerationRate * Time.deltaTime);
        Player.Rigidbody.velocity = velocity;

        Player.HandleFlip(Player.MoveInput.x);

        // Si on pousse vers le bas, applique une force pour plonger plus vite
        if (Player.MoveInput.y < 0)
        {
            Vector3 diveForce = new Vector3(0, Player.MoveInput.y, 0) * Player.Settings.dive;
            Player.Rigidbody.AddForce(diveForce, ForceMode.Acceleration);
        }
    }
}
