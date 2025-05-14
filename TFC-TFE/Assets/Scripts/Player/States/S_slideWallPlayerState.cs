using UnityEngine;

public class S_slideWallPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        Debug.Log("<color=cyan>[WALL]</color> Entered SlideWallState");

        // Active le mode wall slide (gravité désactivée, glisse lente)
        Player.Rigidbody.useGravity = false;
        Player.IsWallSliding = true;

        // Autorise le wall jump
        Player.CanWallJump = true;
        Player.WallJumpTimer = Player.Settings.wallJumpCoyoteTime;

        Debug.Log("<color=yellow>[WALL]</color> Wall jump ready!");
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player)
    {
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

        // Applique une descente contrôlée en glissant sur le mur
        Vector3 vel = Player.Rigidbody.velocity;
        vel.y = -Player.Settings.wallSlideSpeed;
        Player.Rigidbody.velocity = vel;

        // Décompte du temps pour autoriser le wall jump
        if (Player.WallJumpTimer > 0)
            Player.WallJumpTimer -= Time.deltaTime;
    }
}
