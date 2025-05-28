using UnityEngine;

public class S_airDashPlayerState : S_basePlayerStates
{
    //public override void EnterState(S_playerStates Player)
    //{
    //    if (!Player.CanAirDash || Player.IsDashing) return;

    //    Player.IsDashing = true;
    //    Player.CanAirDash = false;
    //    Player.HasAirDashed = true;
    //    Player.IsAirReboundDash = true;

    //    // Calcule la direction du dash en fonction de l'input
    //    Vector3 dashDirection = new Vector3(0, Player.MoveInput.y, Player.MoveInput.x).normalized;

    //    // Si aucune direction, dash vers l'avant
    //    if (dashDirection == Vector3.zero)
    //    {
    //        dashDirection = Player.FacingRight ? Player.transform.forward : -Player.transform.forward;
    //    }

    //    // Lance la coroutine de dash avec gestion du rebond
    //    Player.StartAirDashCoroutine(dashDirection);
    //}

    //public override void OnEnable(S_playerStates Player) { }

    //public override void OnDisable(S_playerStates Player)
    //{
    //    // Réactive la gravité et stoppe les flags liés au dash
    //    Player.Rigidbody.useGravity = true;
    //    Player.IsDashing = false;
    //    Player.IsAirReboundDash = false;
    //}

    //public override void UpdateState(S_playerStates Player) { }

    S_playerManagerStates _player;

    Vector3 _dirAirDash;
    float _strengthAirDash;
    float _durationAirDash;
    public override void EnterState(S_playerManagerStates Player)
    {
        Player.SFX_Dash.Play();

        _player = Player;
        _player.SetfalsePressRebounds();

        _dirAirDash = Player.DashDirection;
        _strengthAirDash = Player.MovementSettings.airDashForce;
        _durationAirDash = Time.time + Player.MovementSettings.AirDashDuration;

        S_controllerPlayer.Instance.ParticleDashManager.StartDashPulse(Player.transform.position,Player.DashDirection);
        Player.SwitchVisual(true);
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
        Player.Rigidbody.velocity = Player.Rigidbody.velocity/ Player.MovementSettings.BrakeAirDashPower;

        
    }
    public override void UpdateState(S_playerManagerStates Player)
    {
        Player.Rigidbody.velocity = _dirAirDash * _strengthAirDash;

        if (Time.time > _durationAirDash)
        {
            _player.SetfalsePressRebounds();
            Player.SwitchState(Player.AirState);
            
        }


        float DistanceRay = ( Mathf.Abs(_dirAirDash.y * Player.MovementSettings.ReboundCorrectionValueCapsule) + 1) * Player.WallCheckDistance;
        RaycastHit hit;

        if (Physics.Raycast(Player.transform.position, _dirAirDash,out hit, DistanceRay, Player.MovementSettings.bounceLayers))
        {
            if (hit.transform.gameObject.layer == Player._invisibleWall) { 
                Player.SwitchState(Player.AirState);
                return;
            }
                
            Player.SwitchState(Player.WallReboundState);
        }


        if (Physics.Raycast(Player.transform.position, _dirAirDash,out hit, DistanceRay, Player.MovementSettings.breakWall))
        {
            if(hit.transform.parent.TryGetComponent<IbreakWall>(out IbreakWall BreakingWall))
            {
                BreakingWall.BreakWall(_player.DashDirection.x);
                S_controllerPlayer.Instance.CameraShake.Shake(3f, 2f);
                S_controllerPlayer.Instance.VibrationGamePad.StartVibration(3f, 2f, 0.2f);
                Player.SwitchState(Player.AirDashState);
                return;
            }
        }

    }

    private void Inputs_OnJumpEvent()
    {
        _player.SetfalsePressRebounds();
        _player.SwitchState(_player.AirJumpState);
    }
    private void Inputs_OnDashEvent()
    {
        _player.StartDureationPushBounds();
    }
}
