using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_ZoneEnd : MonoBehaviour{
    public S_deathAlongueSpline deathSpline;

    private bool WinEnable = false;

    private void OnEnable()
    {
        S_deathAlongueSpline.RestDeadZone += ResetWin;
    }

    private void OnDisable()
    {
        S_deathAlongueSpline.RestDeadZone -= ResetWin;
    }
    private void OnTriggerEnter(Collider other){
        if (!WinEnable && other.CompareTag("Player")){

            WinEnable = true;

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

    private void ResetWin()
    {
        WinEnable = false;
    }
}