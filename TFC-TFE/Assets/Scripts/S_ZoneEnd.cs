using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ZoneEnd : MonoBehaviour{
    public S_deathAlongueSpline deathSpline;
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log("Victoire ! Le joueur a atteint la zone de fin.");
            S_TimerSpeedrun timerSpeedrun = FindObjectOfType<S_TimerSpeedrun>();
            if (timerSpeedrun is not null){
                timerSpeedrun.StopTimer();
            }
            S_controllerPlayer.Instance.VibrationGamePad.StopVibration();
            deathSpline.endWin = true;
            FindObjectOfType<S_GhostManager>()?.OnRunEnd();
            S_playerManagerStates playerManagerStates = other.GetComponent<S_playerManagerStates>();
            playerManagerStates.SwitchState(playerManagerStates.DeadState);
        }
    }
}