using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_FeedbackButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private SoundSystem SFX_Select, SFX_Confirm;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float clickScaleMultiplier = 0.8f;
    [SerializeField] private float animationSpeed = 5f;

    private Vector3 originalScale;
    private bool isSelected = false;
    private bool isClicked = false;
    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    private void Start(){
        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();
        originalScale = transform.localScale;
    }

    private void Update(){
        if (isClicked){
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * clickScaleMultiplier, Time.deltaTime * animationSpeed);
        }else if(isSelected){
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleMultiplier, Time.deltaTime * animationSpeed);
        }else{
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }

    public void OnSelect(BaseEventData eventData){
        if (layoutElement is not null){
            //layoutElement.ignoreLayout = true;
        }
        isSelected = true;
        if(SFX_Select is not null){
            SFX_Select.Play();
        }
    }

    public void OnDeselect(BaseEventData eventData){
        if (layoutElement is not null){
            //layoutElement.ignoreLayout = false;
        }
        isSelected = false;
    }
    public void OnDisable(){
        transform.localScale = originalScale;
        isSelected = false;
        isClicked = false;
        if (SFX_Confirm is not null){
            SFX_Confirm.Play();
        }
    }

}
