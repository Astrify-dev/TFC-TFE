using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class S_PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private GameObject firstMainMenuButton;
    [SerializeField] private GameObject firstSliderOption;
    [SerializeField] private S_CheckpointManager checkpointManager;
    [SerializeField] private S_inputPlayer s_InputPlayer;
    [SerializeField] private S_CanvasEnd s_CanvasEnd;
    [SerializeField] private S_playerManagerStates Player;

    [SerializeField] private SoundSystem SFX_Confirm;
    private bool isPaused = false;

    private void Start(){
        S_inputPlayer inputPlayer = FindObjectOfType<S_inputPlayer>();
        if (inputPlayer is not null){
            inputPlayer.OnPauseToggleEvent += TogglePause;
        }
    }

    private void TogglePause(){
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        s_InputPlayer.PauseGame(isPaused);
    }

    private void OnDestroy(){
        S_inputPlayer inputPlayer = FindObjectOfType<S_inputPlayer>();
        if (inputPlayer is not null){
            inputPlayer.OnPauseToggleEvent -= TogglePause;
        }
    }
    public void OnOptionsButtonPressed(){
        PlayConfirmSoundWithDelay();
        if (mainMenuPanel is not null && optionsPanel is not null)
        {
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSliderOption);
        }
    }
    public void OnQuitButtonPressed()
    {
        PlayConfirmSoundWithDelay();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void OnContinueButtonPressed(){
        PlayConfirmSoundWithDelay();
        TogglePause();
    }

    public void OnRetryPressed() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayConfirmSoundWithDelay();
        Debug.Log("Recommencer le niveau !");
        S_GhostManager ghostManager = FindObjectOfType<S_GhostManager>();
        if (ghostManager is not null)
        {
            ghostManager.OnRunStart();
        }
        S_TimerSpeedrun.OnPlayerDeath?.Invoke();
        S_CanvasEnd.OnPlayerDeath?.Invoke();
        S_TrainReset.OnResetSpline?.Invoke();
        S_wallBreak.OnReset?.Invoke();
        checkpointManager.RespawnPlayer();
        Player._hairEffect.SetActive(false);
        Player._hairEffect.SetActive(true);
        if (isPaused)
            TogglePause();
        S_CanvasEnd.OnPlayerRetry?.Invoke();
        Player.SwitchState(Player.InitialyzePlayerState);
        S_TimerSpeedrun.OnPlayerStart?.Invoke();
        
    }

    public void OnBackButtonPressed(){
        PlayConfirmSoundWithDelay();
        if (optionsPanel is not null && optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
        }

        if (mainMenuPanel is not null)
        {
            mainMenuPanel.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }

    public void PlayConfirmSoundWithDelay(){
        StartCoroutine(PlaySoundAndWaitCoroutine());
    }

    private IEnumerator PlaySoundAndWaitCoroutine(){
        if (SFX_Confirm != null){
            SFX_Confirm.Play();
        }
        yield return new WaitForSeconds(1f); 
    }

}
