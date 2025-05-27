using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

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

    [SerializeField] private float buttonDelay = 0.5f;
    [SerializeField] private SoundSystem SFX_Switch;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float clickScaleMultiplier = 0.8f;
    [SerializeField] private float animationSpeed = 5f;

    private Vector3 originalScale;
    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    private void Start(){
        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();
        originalScale = transform.localScale;
    }

    public void OnPlayButtonPressed(){
        StartCoroutine(DelayedAction(() =>{
            if (!string.IsNullOrEmpty(playSceneName)){
                SceneManager.LoadScene(playSceneName);
            }
        }));
    }
    public void OnOptionsButtonPressed(){
        StartCoroutine(DelayedAction(() =>{
            if (mainMenuPanel is not null && optionsPanel is not null){
                mainMenuPanel.SetActive(false);
                optionsPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstSliderOption);
            }
        }));
    }

    public void OnCreditsButtonPressed(){
        StartCoroutine(DelayedAction(() =>{
            if (mainMenuPanel is not null && creditsPanel is not null){
                mainMenuPanel.SetActive(false);
                creditsPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(backButtonCredits);
            }
        }));
    }

    public void OnQuitButtonPressed(){
        StartCoroutine(DelayedAction(() =>{
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }));
    }

    public void OnBackButtonPressed(){
        StartCoroutine(DelayedAction(() =>{
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
        }));
    }

    private IEnumerator DelayedAction(System.Action action){
        yield return new WaitForSeconds(buttonDelay);
        action?.Invoke(); 
    }

    public void ScaleUp(GameObject button)
    {
        RectTransform buttonTransform = button.GetComponent<RectTransform>();
        if (buttonTransform is not null){
            buttonTransform.localScale = originalScale * scaleMultiplier;
        }
    }

    public void ScaleDown(GameObject button)
    {
        RectTransform buttonTransform = button.GetComponent<RectTransform>();
        if (buttonTransform != null)
        {
            buttonTransform.localScale = originalScale;
        }
    }


}