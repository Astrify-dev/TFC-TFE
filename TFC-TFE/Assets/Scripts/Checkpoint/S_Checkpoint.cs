using UnityEngine;

public class S_Checkpoint : MonoBehaviour{
    [Header("Réapparition")]
    [SerializeField] private Transform respawnPoint; // Point de respawn pour le joueur
    [SerializeField] private Transform deathRespawnPoint; // Point de respawn pour DeathFollow
    [SerializeField] private int deathWaypointIndex = 0;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log("Checkpoint atteint !");
            S_CheckpointManager.instance.SetActiveCheckpoint(this);
        }
    }

    public Transform GetRespawnPoint(){
        return respawnPoint;
    }

    public Transform GetDeathRespawnPoint(){
        return deathRespawnPoint;
    }

    public void RespawnEntities(GameObject player){
        if (respawnPoint is not null){
            player.transform.position = respawnPoint.position;
        }

        S_DeathFollow deathFollow = FindObjectOfType<S_DeathFollow>();
        if (deathFollow is not null && deathRespawnPoint is not null){
            deathFollow.transform.position = deathRespawnPoint.position;
            SetDeathWaypointIndex(deathFollow);
        }
    }

    private void SetDeathWaypointIndex(S_DeathFollow deathFollow){
        deathFollow.CurrentIndex = deathWaypointIndex;
    }

}
