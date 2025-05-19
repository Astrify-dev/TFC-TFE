using UnityEngine;

public class S_groundPlayerState : S_basePlayerStates
{
    S_playerManagerStates _player;
    

    public override void EnterState(S_playerManagerStates Player)
    {
        _player = Player;
        Player.SetAirDash(Player.MovementSettings.GroundAirDashCount);
    }

    public override void OnEnable(S_playerManagerStates Player)
    {
        Player.Inputs.OnJumpEvent += Inputs_OnJumpEvent;
        Player.Inputs.OnDashEvent += Inputs_OnDashEvent;
    }

    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Inputs.OnJumpEvent -= Inputs_OnJumpEvent;
        Player.Inputs.OnDashEvent -= Inputs_OnDashEvent;
    }

    public override void UpdateState(S_playerManagerStates Player)
    {
        if (!Player.CheckGrounded())
        {
            Player.SwitchState(Player.AirState);
            return;
        }

        float moveInput = Player.DirectionInput.x;
        float targetSpeed = moveInput * Player.MovementSettings.maxMoveSpeed;
        Vector3 velocity = Player.Rigidbody.velocity;

        // Modifie uniquement la vitesse horizontale (z)
        velocity.z = Mathf.MoveTowards(velocity.z, targetSpeed, Player.MovementSettings.accelerationRate * Time.deltaTime);
        Player.Rigidbody.velocity = velocity;

        Player.HandleFlip(moveInput);
    }

    private void Inputs_OnJumpEvent()
    {
        _player.SwitchState(_player.JumpState);
    }

    private void Inputs_OnDashEvent()
    {
        if(_player.EnableGroundDash)
            _player.SwitchState(_player.DashGroundState);
    }
}
