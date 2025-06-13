using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
public class S_LolSecret : MonoBehaviour
{
    [SerializeField] Inputs _inputs;
    [SerializeField] Material _fullScreenPath;

    private bool _enabled = false;

    private int _index = 0;

    private void OnEnable()
    {
        _inputs = new Inputs();
        _inputs.Lol.Enable();
        _inputs.Lol.DrunkDown.performed += DrunkDown_performed;
        _inputs.Lol.DrunkUp.performed += GetDrunkUp_performed;
        _inputs.Lol.DrunkLeft.performed += GetDrunkLeft_performed;
        _inputs.Lol.DrunkRight.performed += DrunkRight_performed;
        _enabled = false;
        _index = 0;
        _fullScreenPath.SetInt("_Drunk", 0);
    }



    private void OnDisable()
    {
        _inputs.Lol.Disable();
        _inputs.Lol.DrunkDown.performed -= DrunkDown_performed;
        _inputs.Lol.DrunkUp.performed -= GetDrunkUp_performed;
        _inputs.Lol.DrunkLeft.performed -= GetDrunkLeft_performed;
        _inputs.Lol.DrunkRight.performed -= DrunkRight_performed;
    }

    private void DrunkDown_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_enabled)
        {
            ResetDrunk();
            return;
        }

        if (_index == 2 || _index == 3) _index++;
        else _index = 0;
    }

    private void GetDrunkUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_enabled)
        {
            ResetDrunk();
            return;
        }

        if (_index == 0 || _index == 1) _index++;
        else _index = 0;
    }

    private void GetDrunkLeft_performed(InputAction.CallbackContext context)
    {
        if (_enabled)
        {
            ResetDrunk();
            return;
        }

        if (_index == 4 || _index == 6) _index++;
        else _index = 0;
    }
    private void DrunkRight_performed(InputAction.CallbackContext context)
    {
        if (_enabled)
        {
            ResetDrunk();
            return;
        }

        if (_index == 5) _index++;
        else if (_index == 7)
        {
            _index = 0;
            _enabled = true;
            _fullScreenPath.SetInt("_Drunk", 1);
        }
        else _index = 0;

    }

    private void ResetDrunk()
    {
        _index = 0;
        _fullScreenPath.SetInt("_Drunk", 0);
        _enabled = false;
    }
}
