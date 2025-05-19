using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
