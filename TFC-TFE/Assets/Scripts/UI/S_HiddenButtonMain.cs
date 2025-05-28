using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_HiddenButtonMain : MonoBehaviour{
    [SerializeField] private GameObject _UIHiden;

    private void Start(){
        if (_UIHiden is not null)
            _UIHiden.SetActive(false);
        StartCoroutine(ActivateUIHidenWithDelay());
    }

    private IEnumerator ActivateUIHidenWithDelay(){
        yield return new WaitForSeconds(2.15f);
        _UIHiden.SetActive(true);
    }
}
