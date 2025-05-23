using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_CanvasEnd : MonoBehaviour{
    [SerializeField] private GameObject _canvasGameOver;
    [SerializeField] private GameObject _canvasWin;
    [SerializeField] private GameObject _timerGame;

    [SerializeField] private GameObject _NoBestTime;
    [SerializeField] private GameObject _BestTime;

    [SerializeField] private TextMeshProUGUI _winTimeNewBestText;
    [SerializeField] private TextMeshProUGUI _winTimeLastBestText;
    [SerializeField] private TextMeshProUGUI _winTimeNoBestText;
    public static Action OnPlayerDeath;
    public static Action OnPlayerRetry;

    S_TimerSpeedrun _timerSpeedrun;
    private void OnEnable(){
        OnPlayerDeath += ShowGameOver;
        S_TimerSpeedrun.OnTimerSaved += ShowWin;
        OnPlayerRetry += HideEnd;
    }

    private void OnDisable(){
        OnPlayerDeath -= ShowGameOver;
        S_TimerSpeedrun.OnTimerSaved -= ShowWin;
        OnPlayerRetry -= HideEnd;
    }

    public void Start(){
        _canvasGameOver.SetActive(false);
        _canvasWin.SetActive(false);
    }

    public void ShowGameOver(){
        _canvasGameOver.SetActive(true);
        _timerGame.SetActive(false);
        Time.timeScale = 0;
    }

    public void ShowWin(){
        Time.timeScale = 0;
        _canvasWin.SetActive(true);
        _timerGame.SetActive(false);

        float finalTime = PlayerPrefs.GetFloat("FinalTime");
        float bestTime = PlayerPrefs.GetFloat("BestTime", 6039.999f);

        _winTimeNoBestText.text = $"{FormatTime(finalTime)}";
        _winTimeLastBestText.text = $"{FormatTime(bestTime)}";
        _winTimeNewBestText.text = $"{FormatTime(bestTime)}";
        if (finalTime == bestTime){
            _NoBestTime.SetActive(false);
            _BestTime.SetActive(true);
        }else{
            _NoBestTime.SetActive(true);
            _BestTime.SetActive(false);
        }

        Debug.Log($"Temps final : {FormatTime(finalTime)}, Meilleur temps : {FormatTime(bestTime)}");
    }


    public void HideEnd(){
        _canvasGameOver.SetActive(false);
        _canvasWin.SetActive(false);
        _timerGame.SetActive(true);
        Time.timeScale = 1;
    }

    private string FormatTime(float time){
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }
}
