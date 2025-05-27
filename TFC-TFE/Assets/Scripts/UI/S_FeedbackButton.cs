using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_FeedbackButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private SoundSystem SFX_Switch;
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
            print("CLICKED");
            //transform.localScale = Vector3.Lerp(transform.localScale, originalScale * clickScaleMultiplier, Time.deltaTime * animationSpeed);
        }else if(isSelected){
            print("SELECTED");
            //transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleMultiplier, Time.deltaTime * animationSpeed);
        }else{
            print("NOT SELECTED OR CLICKED");
            //transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }

    public void OnSelect(BaseEventData eventData){
        if (layoutElement is not null){
            //layoutElement.ignoreLayout = true;
        }
        isSelected = true;
        if(SFX_Switch is not null){
            SFX_Switch.Play();
        }       
        print("OnSelect called + "+ isSelected);

    }

    public void OnDeselect(BaseEventData eventData){
        if (layoutElement is not null){
            //layoutElement.ignoreLayout = false;
        }
        isSelected = false;
    }
    private void OnDisable()
    {
        isSelected = false;
        isClicked = false;
    }

    private void OnEnable()
    {
        if (!isSelected && !isClicked)
        {
            transform.localScale = originalScale;
        }
    }

}
