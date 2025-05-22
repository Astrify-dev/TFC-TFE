using Cinemachine;
using UnityEngine;

public class S_groundPlayerState : S_basePlayerStates
{
    S_playerManagerStates _player;
    private float _targetScreenY = 0.9f;
    private Vector3 _velocity;


    public override void EnterState(S_playerManagerStates Player)
    {

        _player = Player;
        Player.SetAirDash(Player.MovementSettings.GroundAirDashCount);
        _velocity = Player.Rigidbody.velocity;

        Player.AnimatorPlayer.SetBool("IsInTheAir", false);
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

        if(_velocity.y < 0)
            _velocity = Player.Rigidbody.velocity.z * Vector3.forward;

        float AccelerationDeltaTime = Player.MovementSettings.accelerationRate * Time.deltaTime;

        _velocity.z = Mathf.MoveTowards(_velocity.z, targetSpeed, AccelerationDeltaTime);
        

        Player.Rigidbody.velocity = _velocity;
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
