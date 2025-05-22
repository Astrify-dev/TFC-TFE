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
    public void OnOptionsButtonPressed()
    {
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
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void OnContinueButtonPressed(){
        TogglePause();
    }

    public void OnRetryPressed() { 
        Debug.Log("Recommencer le niveau !");
        checkpointManager.RespawnPlayer();
        TogglePause();
    }
    public void OnBackButtonPressed(){
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
}
