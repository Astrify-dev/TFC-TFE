using UnityEngine;

public class S_wallDashReboundPlayerState : S_basePlayerStates
{
    //private float _reboundDuration = 0.2f;
    //private float _timer = 0f;

    //public override void EnterState(S_playerStates Player)
    //{
    //    _timer = 0f;

    //    // Dash vers la direction du mur en boostant la vitesse
    //    Vector3 direction = Player.FacingRight ? Player.transform.forward : -Player.transform.forward;
    //    Player.Rigidbody.velocity = direction * Player.Settings.airDashForce * Player.Settings.rebounceBoostMultiplier;

    //    Player.Rigidbody.useGravity = false;
    //    Player.IsDashing = true;
    //}

    //public override void OnEnable(S_playerStates Player) { }

    //public override void OnDisable(S_playerStates Player)
    //{
    //    // Réactive la physique normale
    //    Player.Rigidbody.useGravity = true;
    //    Player.IsDashing = false;
    //    Player.IsAirReboundDash = false;
    //}

    //public override void UpdateState(S_playerStates Player)
    //{
    //    // Rebond automatique pendant une très courte durée, puis retourne à l'air
    //    _timer += Time.deltaTime;
    //    if (_timer >= _reboundDuration)
    //    {
    //        Player.SwitchState(Player.AirState);
    //    }
    //}

    S_playerManagerStates _player;
    float _durationWidowsPress;
    Vector3 _dirAirDash;
    public override void EnterState(S_playerManagerStates Player)
    {
        Player.SFX_Rebound.Play();
        _player = Player;
        _dirAirDash = Player.DashDirection;
        _durationWidowsPress = Time.time + Player.MovementSettings.reboundInputWindow;
        Player.Rigidbody.useGravity = false;
    }

    public override void OnEnable(S_playerManagerStates Player)
    {
        Player.Inputs.OnDashEvent += Inputs_OnDashEvent;
    }
    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Inputs.OnDashEvent -= Inputs_OnDashEvent;
        _player.SetfalsePressRebounds();
        Player.Rigidbody.useGravity = true;
    }
    public override void UpdateState(S_playerManagerStates Player)
    {
        if(_durationWidowsPress < Time.time)
            Player.SwitchState(Player.AirState);

        if (Player.PressRebounds)
        {
            RaycastHit hit;

            float DistanceRay = (Mathf.Abs(_dirAirDash.y * Player.MovementSettings.ReboundCorrectionValueCapsule) + 1) * Player.WallCheckDistance;

            if (Physics.Raycast(Player.transform.position, _dirAirDash,out hit, DistanceRay, Player.MovementSettings.bounceLayers))
            {

                Vector3 reflected = Vector3.Reflect(Player.DashDirection, hit.normal).normalized;
                if (Vector3.Angle(reflected, Vector3.down) < 20f)
                    reflected = new Vector3(reflected.x, -0.5f, reflected.z).normalized;

                Player.HandleFlip(reflected.z);

                S_controllerPlayer.Instance.VibrationGamePad.StartVibration(6f, 0.5f, 0.05f);
                Debug.Log("EnableVibration");
                S_controllerPlayer.Instance.CameraShake.Shake(6f, 0.5f);

                Player.AddAirDash(Player.MovementSettings.WallDashMaxAirDashCount);
                Player.DashDirection = reflected;
                Player.SwitchState(Player.AirDashState);
                return;

                
            }
            Player.SwitchState(Player.AirState);

        }

    }



    private void Inputs_OnDashEvent()
    {
        _player.StartDureationPushBounds();
    }
}
