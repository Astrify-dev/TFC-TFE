using UnityEngine;

public class S_airDashPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        if (!Player.CanAirDash || Player.IsDashing) return;

        Player.IsDashing = true;
        Player.CanAirDash = false;
        Player.HasAirDashed = true;
        Player.IsAirReboundDash = true;

        // Calcule la direction du dash en fonction de l'input
        Vector3 dashDirection = new Vector3(0, Player.MoveInput.y, Player.MoveInput.x).normalized;

        // Si aucune direction, dash vers l'avant
        if (dashDirection == Vector3.zero)
        {
            dashDirection = Player.FacingRight ? Player.transform.forward : -Player.transform.forward;
        }

        // Lance la coroutine de dash avec gestion du rebond
        Player.StartAirDashCoroutine(dashDirection);
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player)
    {
        // Réactive la gravité et stoppe les flags liés au dash
        Player.Rigidbody.useGravity = true;
        Player.IsDashing = false;
        Player.IsAirReboundDash = false;
    }

    public override void UpdateState(S_playerStates Player) { }
}
