using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_TimerSpeedrun : MonoBehaviour{
    public static Action OnTimerSaved;
    [SerializeField] private TextMeshProUGUI timerText;
    private bool isRunning;
    private float finalTime;
    public float FinalTime => finalTime;
    public static Action OnPlayerDeath;
    public static Action OnPlayerStart;

    float StartTimer;
    private void OnEnable(){
        OnPlayerDeath += PlayerDeath;
        OnPlayerStart += ResetTimer;
    }

    private void OnDisable(){
        OnPlayerDeath -= PlayerDeath;
        OnPlayerStart -= ResetTimer;
    }

    private void Start(){
        isRunning = true;
        StartTimer = Time.time;
    }

    private void Update(){
        if (isRunning){
            timerText.text = FormatTime(Time.time - StartTimer);
        }
    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }

    public void PlayerDeath(){
        isRunning = false;
        ResetTimer();
    }

    public void StopTimer(){
        isRunning = false;
        finalTime = Time.time - StartTimer;

        PlayerPrefs.SetFloat("FinalTime", finalTime);

        float bestTime = PlayerPrefs.GetFloat("BestTime", 6039.999f);
        if (finalTime < bestTime){
            PlayerPrefs.SetFloat("BestTime", finalTime);
            Debug.Log($"Nouveau meilleur temps : {FormatTime(finalTime)}");
        }

        PlayerPrefs.Save();
        Debug.Log($"Timer arrêté. Temps final : {FormatTime(finalTime)}");

        OnTimerSaved?.Invoke();
    }


    public void ResetTimer(){
        StartTimer = Time.time;
        timerText.text = "00:00:000";
        isRunning = true;
    }
}