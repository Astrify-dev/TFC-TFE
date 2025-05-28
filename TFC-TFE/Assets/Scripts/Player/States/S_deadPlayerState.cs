using UnityEngine;

public class S_deadPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerManagerStates Player){
        Player.SlowMotion.StopSlowMotion();
        Player.SFX_Death.Play();
        Player.Rigidbody.velocity = Vector3.zero;
        Player.Rigidbody.isKinematic = true;
        Player.Inputs.OnDisable();



        Player.AnimatorPlayer.SetTrigger("IsDead");
    }

    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Rigidbody.isKinematic = false;
        Player.Inputs.OnEnable();
    }

    public override void OnEnable(S_playerManagerStates Player)
    {
    }

    public override void UpdateState(S_playerManagerStates Player)
    {
    }
}
