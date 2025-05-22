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

        Time.timeScale = isPaused ? 0 : 1;
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
    public void OnContinueButtonPressed()
    {
        if (pauseMenu != null)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1; // Reprend le temps
        }
    }

    public void OnRetryPressed() { 
        Debug.Log("Recommencer le niveau !");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
