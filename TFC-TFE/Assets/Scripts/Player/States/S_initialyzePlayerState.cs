﻿using UnityEngine;

public class S_initialyzePlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerManagerStates Player)
    {
        // État de démarrage → bascule immédiatement vers le sol
        S_controllerPlayer.Instance.SlowMotionEffect.ResetSlowMotion();
        Player.SwitchVisual(false);
        Player.HandleFlip(1);
        Player.SwitchState(Player.GroundState);
    }

    public override void OnEnable(S_playerManagerStates Player) { }
    public override void OnDisable(S_playerManagerStates Player) { }
    public override void UpdateState(S_playerManagerStates Player) { }
}
