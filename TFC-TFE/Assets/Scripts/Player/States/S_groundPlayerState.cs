using Cinemachine;
using UnityEngine;

public class S_groundPlayerState : S_basePlayerStates
{
    S_playerManagerStates _player;
    private float _targetScreenY = 0.9f;


    public override void EnterState(S_playerManagerStates Player)
    {
        _player = Player;
        Player.SetAirDash(Player.MovementSettings.GroundAirDashCount);

        GroundRebound();
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

    private void GroundRebound()
    {
        float MinRebounds = _player.MovementSettings.JuiceFallVelocityMinRebounds;
        float MultiplyRebounds = _player.MovementSettings.JuiceMultiplyerFallVelocityRebounds;

        if (_player.Rigidbody.velocity.y < -MinRebounds)
        {
            _player.Rigidbody.AddForce(Vector3.up * -_player.Rigidbody.velocity.y * MultiplyRebounds, ForceMode.Impulse);
        }
    }
}
