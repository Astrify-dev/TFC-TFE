using UnityEngine;

public class S_dashGroundPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        Player.IsDashing = true;
        Player.CanDash = false;

        // Calcule la direction du dash (en avant ou en arri�re)
        Vector3 dashDir = Player.FacingRight ? Player.transform.forward : -Player.transform.forward;

        // Applique une force d'impulsion pour simuler un dash imm�diat
        Player.Rigidbody.AddForce(dashDir * Player.Settings.groundDashForce, ForceMode.Impulse);

        // Retour imm�diat � l'�tat de sol apr�s l'impulsion
        Player.SwitchState(Player.GroundState);
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player)
    {
        Player.IsDashing = false;
    }

    public override void UpdateState(S_playerStates Player) { }
}
