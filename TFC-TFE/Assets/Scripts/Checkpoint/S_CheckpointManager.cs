using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class S_CheckpointManager : SingletonBehaviour<S_CheckpointManager>{
    private S_Checkpoint _activeCheckpoint;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform deathStartPosition;

    public void SetActiveCheckpoint(S_Checkpoint checkpoint){
        _activeCheckpoint = checkpoint;
        Debug.Log($"Checkpoint actif défini : {checkpoint.name}");
    }

    public Transform GetPlayerRespawnPoint(){
        if (_activeCheckpoint is not null && _activeCheckpoint.GetRespawnPoint() is not null)
        {
            return _activeCheckpoint.GetRespawnPoint();
        }else if (startPosition is not null){
            return startPosition;
        }else{
            return null;
        }
    }

    public Transform GetDeathRespawnPoint(){
        if (_activeCheckpoint is not null && _activeCheckpoint.GetDeathRespawnPoint() is not null){
            return _activeCheckpoint.GetDeathRespawnPoint();
        }else if (deathStartPosition is not null){
            return deathStartPosition;
        }else{
            return null;
        }
    }

    public void RespawnPlayer(){
        Transform respawnPoint = GetPlayerRespawnPoint();
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
        }else{
            Debug.LogWarning("Aucun point de respawn trouvé !");
        }
    }
}
