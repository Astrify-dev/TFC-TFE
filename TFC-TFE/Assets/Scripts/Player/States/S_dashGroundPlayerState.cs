using UnityEngine;

public class S_dashGroundPlayerState : S_basePlayerStates
{

    //public override void UpdateState(S_playerStates Player) { }
    public override void EnterState(S_playerManagerStates Player)
    {
        Player.SFX_Dash.Play();
        if(Player.MovementSettings.groundDashCooldown)
            Player.StartReloadGroundDash();

        // Calcule la direction du dash (en avant ou en arrière)
        Vector3 dashDir = Player.FacingRight ? Player.transform.forward : -Player.transform.forward;

        // Applique une force d'impulsion pour simuler un dash immédiat
        //Player.Rigidbody.AddForce(dashDir * Player.MovementSettings.groundDashForce, ForceMode.Impulse);
        Player.Rigidbody.velocity = dashDir * Player.MovementSettings.groundDashForce;

        // Retour immédiat à l'état de sol après l'impulsion
        Player.SwitchState(Player.GroundState);

        Player.AnimatorPlayer.SetTrigger("Dash");
        Player._groundDashParticles.Play();
    }

    public override void OnDisable(S_playerManagerStates Player)
    {
    }

    public override void OnEnable(S_playerManagerStates Player)
    {
    }

    public override void UpdateState(S_playerManagerStates Player)
    {
    }
    
}
