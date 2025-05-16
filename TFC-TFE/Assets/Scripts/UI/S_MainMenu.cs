using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class S_MainMenu : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private string playSceneName; 

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel; 
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private GameObject firstMainMenuButton;
    [SerializeField] private GameObject firstSliderOption;
    [SerializeField] private GameObject backButtonCredits;

    public void OnPlayButtonPressed(){
        if (!string.IsNullOrEmpty(playSceneName)){
            SceneManager.LoadScene(playSceneName);
        }
    }

    public void OnOptionsButtonPressed(){
        if (mainMenuPanel is not null && optionsPanel is not null){
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSliderOption);
        }
    }

    public void OnCreditsButtonPressed(){
        if (mainMenuPanel is not null && creditsPanel is not null){
            mainMenuPanel.SetActive(false);
            creditsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backButtonCredits);
        }
    }

    public void OnQuitButtonPressed(){
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void OnBackButtonPressed(){
        if (optionsPanel is not null && optionsPanel.activeSelf){
            optionsPanel.SetActive(false);
        }

        if (creditsPanel is not null && creditsPanel.activeSelf){
            creditsPanel.SetActive(false);
        }

        if (mainMenuPanel is not null){
            mainMenuPanel.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }
}
