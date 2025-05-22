using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_FocusButton : MonoBehaviour
{
    [SerializeField] private GameObject focusButton;
    private void OnEnable(){
        SetFocus();
    }

    private void Update(){
        if (EventSystem.current.currentSelectedGameObject is null){
            SetFocus();
        }
    }

    private void SetFocus(){
        EventSystem.current.SetSelectedGameObject(focusButton);
    }
}
