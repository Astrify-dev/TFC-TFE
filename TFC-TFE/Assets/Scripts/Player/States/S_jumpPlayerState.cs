using UnityEngine;

public class S_JumpPlayerState : S_basePlayerStates
{
    S_playerManagerStates _player;
    float _jumpDuration;
    float _jumpSpeedDuration;
    public override void EnterState(S_playerManagerStates Player)
    {
        _jumpSpeedDuration = 1 / Player.MovementSettings.jumpChargeTime;
        _player = Player;
        _jumpDuration = 0;

        Player.AnimatorPlayer.SetTrigger("JumpStart");

    }

    public override void OnEnable(S_playerManagerStates Player)
    {
        Player.Inputs.OnJumpReleased += Inputs_OnJumpReleased;
        Player.Inputs.OnDashEvent += Inputs_OnDashEvent;
    }
    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Inputs.OnJumpReleased -= Inputs_OnJumpReleased;
        Player.Inputs.OnDashEvent -= Inputs_OnDashEvent;
    }
    public override void UpdateState(S_playerManagerStates Player)
    {
        _jumpDuration += Time.deltaTime * _jumpSpeedDuration;

        if(_jumpDuration > 1)
            _player.SwitchState(_player.AirState);

        float ForceJump = Mathf.Lerp(Player.MovementSettings.maxJumpForce, Player.MovementSettings.minJumpForce, _jumpDuration);

        Player.Rigidbody.AddForce(Vector3.up * ForceJump * Time.deltaTime , ForceMode.Impulse);


        Vector3 VelocityDir = new Vector3(0, 0, Player.DirectionInput.x);

        if(Player.MovementSettings.AirControlEnable)
            Player.Rigidbody.AddForce(VelocityDir * Player.MovementSettings.airMaxMoveSpeed * Time.deltaTime, ForceMode.Acceleration);
    }

    private void Inputs_OnJumpReleased()
    {
        _player.SwitchState(_player.AirState);
    }

    private void Inputs_OnDashEvent()
    {
        if(_player.AirDashCount > 0)
            _player.SwitchState(_player.SlowMotionDashState);
    }
}
