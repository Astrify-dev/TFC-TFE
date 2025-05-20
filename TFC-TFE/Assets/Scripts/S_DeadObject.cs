using UnityEngine;

public class S_DeadObject : MonoBehaviour{
    [Header("Paramètres de mort")]
    [SerializeField] private bool isGameOverOnDeath = false;

    private void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("Player")){
            Debug.Log("Game Over !");
            /*S_playerStates playerStates = collision.collider.GetComponent<S_playerStates>();
            if (playerStates is not null){
                if (isGameOverOnDeath){
                    //playerStates.SwitchState(playerStates.DeadState);
                }else{
                    RespawnPlayer();
                }
            }*/
        }
    }

    private void RespawnPlayer(){
        Transform respawnPoint = S_CheckpointManager.instance.GetPlayerRespawnPoint();
        if (respawnPoint is not null){
            GameObject player = GameObject.FindWithTag("Player");
            if (player is not null){
                player.transform.position = respawnPoint.position;
            }
            S_DeathFollow deathFollow = FindObjectOfType<S_DeathFollow>();
            if (deathFollow is not null){
                Transform deathRespawnPoint = S_CheckpointManager.instance.GetDeathRespawnPoint();
                if (deathRespawnPoint is not null){
                    deathFollow.transform.position = deathRespawnPoint.position;
                }
            }
        }
    }
}
