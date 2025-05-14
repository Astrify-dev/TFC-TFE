using UnityEngine;

public class S_initialyzePlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        // État de démarrage → bascule immédiatement vers le sol
        Player.SwitchState(Player.GroundState);
    }

    public override void OnEnable(S_playerStates Player) { }
    public override void OnDisable(S_playerStates Player) { }
    public override void UpdateState(S_playerStates Player) { }
}
