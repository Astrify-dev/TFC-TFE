using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_TimerSpeedrun : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime;
    private bool isRunning;
    private float finalTime;
    public float FinalTime => finalTime;
    public static Action OnPlayerDeath;
    public static Action OnPlayerWin;
    public static Action OnPlayerStart;

    private void OnEnable(){
        OnPlayerDeath += StopTimer;
        OnPlayerWin += StopTimer;
        OnPlayerStart += ResetTimer;
    }

    private void OnDisable(){
        OnPlayerDeath -= StopTimer;
        OnPlayerWin -= StopTimer;
        OnPlayerStart -= ResetTimer;
    }

    private void Start(){
        isRunning = true;
        elapsedTime = 0f;
    }

    private void Update(){
        if (isRunning){
            elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(elapsedTime);
        }
    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }

    public void StopTimer(){
        isRunning = false;
        finalTime = elapsedTime;
        Debug.Log($"Timer arrêté. Temps final : {FormatTime(finalTime)}");
    }

    public void ResetTimer(){
        elapsedTime = 0f;
        timerText.text = "00:00:000";
        isRunning = true;
    }
}