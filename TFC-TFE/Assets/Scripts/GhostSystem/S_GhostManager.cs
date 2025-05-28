using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GhostManager : MonoBehaviour
{
    public S_GhostRecorder recorder; // attach� au joueur
    public S_GhostPlayer ghostPlayer; // attach� au prefab du fant�me
    public GameObject ghostObject; // objet � activer au d�but du 2e run
    public GameTimer gameTimer;
    private List<S_GhostFrame> lastRun;

    private void Start()
    {
        OnRunStart();
    }
    public void OnRunStart()
    {
        recorder.ResetRecording();
        gameTimer.StartTimer();
        if (lastRun != null && lastRun.Count > 0)
        {
            ghostObject.SetActive(true);
            ghostPlayer.StartReplay(lastRun);
        }
        else
        {
            ghostObject.SetActive(false);
        }
    }

    public void OnRunEnd()
    {
        gameTimer.StopTimer();
        lastRun = new List<S_GhostFrame>(recorder.RecordedFrames);
    }

    public void SetGhostPause(bool isPaused){
        if (recorder != null)
            recorder.enabled = !isPaused; 

        if (gameTimer != null)
            gameTimer.isRunning = !isPaused; 

        if (ghostPlayer != null)
            ghostPlayer.SetPause(isPaused); 
    }

}