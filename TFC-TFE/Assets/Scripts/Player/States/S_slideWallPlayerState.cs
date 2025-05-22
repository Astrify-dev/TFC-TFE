using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_slideWallPlayerState : S_basePlayerStates
{
    //public override void EnterState(S_playerStates Player)
    //{

    //    // Active le mode wall slide (gravité désactivée, glisse lente)
    //    Player.Rigidbody.useGravity = false;
    //    Player.IsWallSliding = true;

    //    // Autorise le wall jump
    //    Player.CanWallJump = true;

    //}

    //public override void OnEnable(S_playerStates Player) { }

    //public override void OnDisable(S_playerStates Player)
    //{
    //    Player.Rigidbody.useGravity = true;
    //    Player.IsWallSliding = false;
    //}

    //public override void UpdateState(S_playerStates Player)
    //{
    //    if (Player.CheckGrounded())
    //    {
    //        Player.SwitchState(Player.GroundState);
    //        return;
    //    }

    //    if (!Player.CheckWall())
    //    {
    //        Player.SwitchState(Player.AirState);
    //        return;
    //    }

    //    // Applique une descente contrôlée en glissant sur le mur
    //    Vector3 vel = Player.Rigidbody.velocity;
    //    vel.y = -Player.Settings.wallSlideSpeed;
    //    Player.Rigidbody.velocity = vel;

    //    // Décompte du temps pour autoriser le wall jump
    //    if (Player.WallJumpTimer > 0)
    //        Player.WallJumpTimer -= Time.deltaTime;
    //}

    private S_playerManagerStates _player;
    public override void EnterState(S_playerManagerStates Player)
    {
        // Active le mode wall slide (gravité désactivée, glisse lente)
        _player = Player;
        Player.Rigidbody.useGravity = false;

        Player.AnimatorPlayer.SetBool("IsLandingWall", true);
    }
    public override void OnEnable(S_playerManagerStates Player)
    {
        Player.Inputs.OnJumpEvent += Inputs_OnJumpEvent;
    }
    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Rigidbody.useGravity = true;
        Player.Inputs.OnJumpEvent -= Inputs_OnJumpEvent;

        Player.AnimatorPlayer.SetBool("IsLandingWall", false);
    }
    public override void UpdateState(S_playerManagerStates Player)
    {
        if (Player.CheckGrounded())
        {
            Player.SwitchState(Player.GroundState);
            return;
        }

        if (!Player.CheckWall(Player.WallCheckDistance))
        {
            Player.SwitchState(Player.AirState);
            return;
        }


        // Applique une descente contrôlée en glissant sur le mur
        Vector3 vel = Player.Rigidbody.velocity;
        vel.y = -Player.MovementSettings.wallSlideSpeed;
        Player.Rigidbody.velocity = vel;

        Player.HandleFlip(Player.DirectionInput.x);

    }

    private void Inputs_OnJumpEvent()
    {
        _player.SwitchState(_player.WallJumpState);
    }
}
