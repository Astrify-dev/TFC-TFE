using UnityEngine;

public class S_deadPlayerState : S_basePlayerStates
{
    public override void EnterState(S_playerStates Player)
    {
        // Stoppe tout mouvement
        Player.Rigidbody.velocity = Vector3.zero;

        // Bloque le Rigidbody pour �viter toute interaction physique
        Player.Rigidbody.isKinematic = true;
    }

    public override void OnEnable(S_playerStates Player) { }

    public override void OnDisable(S_playerStates Player)
    {
        // R�active la physique si le joueur revient � la vie
        Player.Rigidbody.isKinematic = false;
    }

    public override void UpdateState(S_playerStates Player) { }
}
