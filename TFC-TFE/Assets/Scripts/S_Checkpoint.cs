using System;
using UnityEngine;

public class S_Checkpoint : MonoBehaviour{
    [Header("Réapparition")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Transform deathRespawnPoint;
    [SerializeField] private int deathWaypointIndex = 0;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log("Checkpoint atteint !");
            S_DeathFollow.OnPlayerDead += () => RespawnEntities(other.gameObject);
        }
    }

    private void RespawnEntities(GameObject player){
        if (respawnPoint is not null){
            Debug.Log("Réapparition du joueur au checkpoint.");
            player.transform.position = respawnPoint.position;
        }

        S_DeathFollow deathFollow = FindObjectOfType<S_DeathFollow>();
        if (deathFollow is not null && deathRespawnPoint is not null){
            Debug.Log("Réapparition de la mort au point défini.");
            deathFollow.transform.position = deathRespawnPoint.position;

            SetDeathWaypointIndex(deathFollow);
        }
    }

    private void SetDeathWaypointIndex(S_DeathFollow deathFollow){
        var waypointsField = typeof(S_DeathFollow).GetField("currentIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (waypointsField is not null){
            waypointsField.SetValue(deathFollow, deathWaypointIndex);
        }
    }

    private void OnDestroy(){
        S_DeathFollow.OnPlayerDead -= () => RespawnEntities(null);
    }
}
