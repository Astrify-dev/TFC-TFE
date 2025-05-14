using UnityEngine;

public class S_slideWallPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        // Désactive la gravité pour que le glissement soit contrôlé manuellement
        Player.Rigidbody.useGravity = false;
        Player.IsWallSliding = true;
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player)
    {
        // Réactive la gravité à la sortie du wall slide
        Player.Rigidbody.useGravity = true;
        Player.IsWallSliding = false;
    }

    public override void UpdateState(S_playerStates Player)
    {
        if (Player.CheckGrounded())
        {
            Player.SwitchState(Player.GroundState);
            return;
        }

        if (!Player.CheckWall())
        {
            Player.SwitchState(Player.AirState);
            return;
        }

        // Contrôle manuel de la descente à une vitesse constante
        Vector3 vel = Player.Rigidbody.velocity;
        vel.y = -Player.Settings.wallSlideSpeed;
        Player.Rigidbody.velocity = vel;
    }
}
