using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_CanvasEnd : MonoBehaviour{
    [SerializeField] private GameObject _canvasGameOver;
    [SerializeField] private GameObject _canvasWin;
    [SerializeField] private TextMeshProUGUI _winTimeText;
    public static Action OnPlayerDeath;
    public static Action OnPlayerWinTimer;
    public static Action OnPlayerRetry;

    S_TimerSpeedrun _timerSpeedrun;
    private void OnEnable(){
        OnPlayerDeath += ShowGameOver;
        OnPlayerWinTimer += ShowWin;
        OnPlayerRetry += HideEnd;
    }

    private void OnDisable(){
        OnPlayerDeath -= ShowGameOver;
        OnPlayerWinTimer -= ShowWin;
        OnPlayerRetry -= HideEnd;
    }

    public void Start(){
        _canvasGameOver.SetActive(false);
        _canvasWin.SetActive(false);
    }

    public void ShowGameOver(){
        _canvasGameOver.SetActive(true);
        Time.timeScale = 0;
    }   

    public void ShowWin(){
        Time.timeScale = 0;
        _canvasWin.SetActive(true);
        _winTimeText.text = $"Temps final : {FormatTime(_timerSpeedrun.FinalTime)}";

    }

    public void HideEnd(){
        _canvasGameOver.SetActive(false);
        _canvasWin.SetActive(false);
        Time.timeScale = 1;
    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }
}
