using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_inputPlayer : MonoBehaviour{
    public Inputs _inputs;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;
    public event Action OnJumpReleased;
    public event Action OnDashEvent;
    public event Action OnDashReleased;
    public event Action OnPauseToggleEvent;


    private bool _moveEnabled = true;
    private bool _jumpEnabled = true;
    private bool _dashEnabled = true;
    private bool _pauseEnabled = true;

    private bool isPaused = false;

    public bool IsJumpHeld => _inputs.Player.Jump.IsPressed();

    private void Awake(){
        _inputs = new Inputs();
    }

    public void OnEnable(){
        _inputs.Player.Enable();
        _inputs.Player.Direction.performed += OnMove;
        _inputs.Player.Direction.canceled += OnMove;
        _inputs.Player.Jump.performed += OnJump;
        _inputs.Player.Jump.canceled += OnJumpRelease;
        _inputs.Player.Dash.performed += OnDash;
        _inputs.Player.Dash.canceled += OnDashRelease;
        _inputs.Player.Pause.performed += OnPauseToggle;

    }

    public void OnDisable(){
        _inputs.Player.Disable();
        _inputs.Player.Direction.performed -= OnMove;
        _inputs.Player.Direction.canceled -= OnMove;
        _inputs.Player.Jump.performed -= OnJump;
        _inputs.Player.Jump.canceled -= OnJumpRelease;
        _inputs.Player.Dash.performed -= OnDash;
        _inputs.Player.Dash.canceled -= OnDashRelease;
        _inputs.Player.Pause.performed -= OnPauseToggle;
    }

    private void OnMove(InputAction.CallbackContext context){
        if (_moveEnabled){
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(moveInput);
        }
    }

    private void OnJump(InputAction.CallbackContext context){
        if (_jumpEnabled){
            OnJumpEvent?.Invoke();
        }
    }

    private void OnJumpRelease(InputAction.CallbackContext context)
    {
        if (_jumpEnabled)
            OnJumpReleased?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_dashEnabled)
        {
            OnDashEvent?.Invoke();
        }
    }


    private void OnDashRelease(InputAction.CallbackContext context){
        if (_dashEnabled){
            OnDashReleased?.Invoke();
        }
    }

    private void OnPauseToggle(InputAction.CallbackContext context){
        //PauseGame(!isPaused);
        OnPauseToggleEvent?.Invoke();
    }

    public void PauseGame(bool pause){
        if (_pauseEnabled){
            isPaused = pause;

            EnableMove(!isPaused);
            EnableJump(!isPaused);
            EnableDash(!isPaused);       
            Time.timeScale = isPaused ? 0 : 1;
            
        }
    }

    public void EnableMove(bool enable) => _moveEnabled = enable;
    public void EnableJump(bool enable) => _jumpEnabled = enable;
    public void EnableDash(bool enable) => _dashEnabled = enable;
    public void EnablePause(bool enable) => _pauseEnabled = enable;

}
