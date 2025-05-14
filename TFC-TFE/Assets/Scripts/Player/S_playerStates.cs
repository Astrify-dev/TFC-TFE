using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_playerStates : MonoBehaviour
{

    S_basePlayerStates _currentState;

    //class states
    public S_initialyzePlayerState _initialyzePlayerState { get; private set; } = new S_initialyzePlayerState();

    private void Start()
    {
        _currentState = _initialyzePlayerState;
        _currentState?.EnterState(this);
        _currentState?.OnEnable(this);
    }

    private void OnEnable()
    {
        _currentState = _initialyzePlayerState;
        _currentState?.EnterState(this);
        _currentState?.OnEnable(this);
    }

    private void OnDisable()
    {
        _currentState?.OnDisable(this);
    }

    private void Update()
    {
        _currentState?.UpdateState(this);
    }

    public void SwitchState(S_basePlayerStates state)
    {
        _currentState?.OnDisable(this);
        _currentState = state;
        state?.OnEnable(this);
        state?.EnterState(this);
    }

}
