using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class S_LolSecret : MonoBehaviour
{
    [SerializeField] Inputs _inputs;
    [SerializeField] Material _fullScreenPath;

    private bool _enabled = false;

    private void OnEnable()
    {
        _inputs = new Inputs();
        _inputs.Lol.Enable();
        _inputs.Lol.Drunk.performed += Drunk_performed;
        _enabled = false;
        _fullScreenPath.SetInt("_Drunk", 0);
    }

    private void OnDisable()
    {
        _inputs.Lol.Disable();
        _inputs.Lol.Drunk.performed -= Drunk_performed;
    }

    private void Drunk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _enabled = !_enabled;
        _fullScreenPath.SetInt("_Drunk", _enabled? 1:0);
    }
}
