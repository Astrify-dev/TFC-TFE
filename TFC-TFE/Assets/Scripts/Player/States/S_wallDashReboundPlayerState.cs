using UnityEngine;

public class S_wallDashReboundPlayerState : S_basePlayerStates
{
    private float _reboundDuration = 0.2f;
    private float _timer = 0f;

    public override void EnterState(S_playerStates Player)
    {
        _timer = 0f;

        // Dash vers la direction du mur en boostant la vitesse
        Vector3 direction = Player.FacingRight ? Player.transform.forward : -Player.transform.forward;
        Player.Rigidbody.velocity = direction * Player.Settings.airDashForce * Player.Settings.rebounceBoostMultiplier;

        Player.Rigidbody.useGravity = false;
        Player.IsDashing = true;
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player)
    {
        // Réactive la physique normale
        Player.Rigidbody.useGravity = true;
        Player.IsDashing = false;
        Player.IsAirReboundDash = false;
    }

    public override void UpdateState(S_playerStates Player)
    {
        // Rebond automatique pendant une très courte durée, puis retourne à l'air
        _timer += Time.deltaTime;
        if (_timer >= _reboundDuration)
        {
            Player.SwitchState(Player.AirState);
        }
    }
}
