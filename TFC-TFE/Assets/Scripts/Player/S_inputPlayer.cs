using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_inputPlayer : MonoBehaviour{
    public Inputs _inputs;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnJumpEvent;
    public event Action OnDashEvent;

    private bool _moveEnabled = true;
    private bool _jumpEnabled = true;
    private bool _dashEnabled = true;
    
    private void Awake(){
        _inputs = new Inputs();
    }

    private void OnEnable(){
        _inputs.Player.Enable();
        _inputs.Player.Direction.performed += OnMove;
        _inputs.Player.Direction.canceled += OnMove;
        _inputs.Player.Jump.performed += OnJump;
        _inputs.Player.Dash.performed += OnDash;
    }

    private void OnDisable(){
        _inputs.Player.Disable();
        _inputs.Player.Direction.performed -= OnMove;
        _inputs.Player.Direction.canceled -= OnMove;
        _inputs.Player.Jump.performed -= OnJump;
        _inputs.Player.Dash.performed -= OnDash;
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

    private void OnDash(InputAction.CallbackContext context){
        if (_dashEnabled){
            OnDashEvent?.Invoke();
        }
    }

    public void EnableMove(bool enable) => _moveEnabled = enable;
    public void EnableJump(bool enable) => _jumpEnabled = enable;
    public void EnableDash(bool enable) => _dashEnabled = enable;

}
