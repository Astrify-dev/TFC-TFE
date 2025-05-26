using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float currentTime { get; private set; } = 0f;

    public S_GhostRecorder ghostRecorder;
    public S_GhostPlayer ghostPlayer;

    public bool isRunning = false;

    void Update()
    {
        if (!isRunning) return;

        // Utilise le vrai temps, pas Time.deltaTime (qui est affecté par timeScale)
        float deltaTime = Time.unscaledDeltaTime;
        currentTime += deltaTime;

        ghostRecorder?.RecordFrame(currentTime);
        ghostPlayer?.UpdateGhost(currentTime);
    }

    public void StartTimer()
    {
        currentTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}