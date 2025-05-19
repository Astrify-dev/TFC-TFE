using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_slowMotionDashPlayerState : S_basePlayerStates
{

    S_playerManagerStates _player;
    Vector3 _dirAirDash;
    float _slowMotionDuration;
    public override void EnterState(S_playerManagerStates Player)
    {
        _player = Player;
        

        float speed = Player.MovementSettings.SlowMotionAnimSpeed;
        float Intensity = Player.MovementSettings.slowMotionIntensity;
        AnimationCurve Curve = Player.MovementSettings.SlowMotionAnimEnableCurve;

        Player.SlowMotion.StartSlowMotionAnimEnable(speed, Intensity, Curve);


        _slowMotionDuration = Time.unscaledTime + Player.MovementSettings.slowMotionTimer;
    }
    public override void OnEnable(S_playerManagerStates Player)
    {
        Player.Inputs.OnDashReleased += Inputs_OnDashReleased;
    }
    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Inputs.OnDashReleased -= Inputs_OnDashReleased;
    }

    public override void UpdateState(S_playerManagerStates Player)
    {
        _dirAirDash = new Vector3(0,Player.DirectionInput.y, Player.DirectionInput.x);

        if (_dirAirDash == Vector3.zero)
            _dirAirDash = Player.FacingRight? Vector3.forward : Vector3.back;

        if (Player.CheckGrounded())
        {
            ResetTimeScale();
            Player.SwitchState(Player.GroundState);
        }

        if(Time.unscaledTime > _slowMotionDuration)
        {
            ResetTimeScale();
            Player.SwitchState(Player.AirState);
        }


    }
    private void ResetTimeScale()
    {
        _player.SlowMotion.StopSlowMotionAnimEnable();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
    private void Inputs_OnDashReleased()
    {
        ResetTimeScale();
        _player.DashDirection = _dirAirDash;
        _player.AddAirDash(-1);
        _player.SwitchState(_player.AirDashState);
    }
}
