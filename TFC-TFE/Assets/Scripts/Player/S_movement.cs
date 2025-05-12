using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using System;

public class S_movement : MonoBehaviour{
    [Header("Paramètres de déplacement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _dashForce = 10f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private bool _canJump = true;

    [Header("Paramètres physiques")]
    [SerializeField] private Rigidbody _rb;

    [Header("Paramètres de rotation")]
    [SerializeField] private GameObject _objectToFlip;

    [Header("Paramètres de slowmotion")]
    [SerializeField] private float _intensiteSlow = 0.5f;
    [SerializeField] private float _timerSlow = 10f;

    private bool _isGrounded = true;
    private bool _isDashing = false;
    private bool _facingRight = true;

    private Vector2 _moveInput;

    private bool _jumpInput;
    private bool _dashInput;

    private Inputs _inputs;

    private void Awake(){
        _inputs = new Inputs();

        if (_rb is null){
            _rb = GetComponent<Rigidbody>();
        }
        if (_objectToFlip is null){
            _objectToFlip = gameObject;
        }
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
        _inputs.Player.Direction.performed -= OnMove ;
        _inputs.Player.Direction.canceled -= OnMove;
        _inputs.Player.Jump.performed -= OnJump;
        _inputs.Player.Dash.performed -= OnDash;
    }

    private void OnMove(InputAction.CallbackContext context){
        _moveInput = Vector2.MoveTowards(_moveInput, context.ReadValue<Vector2>(), Time.deltaTime * (_moveSpeed*100));
    }

    private void OnJump(InputAction.CallbackContext context){
        if (_isGrounded && _canJump){
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void OnDash(InputAction.CallbackContext context){
        if (!_isDashing){
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate(){
        if (_isDashing) return;

        float moveInput = _moveInput.x;
        Vector3 move = new Vector3(0, 0, moveInput * _moveSpeed);
        _rb.velocity = new Vector3(0, _rb.velocity.y, move.z);

        if (moveInput > 0 && !_facingRight){
            Flip(0);
        }else if (moveInput < 0 && _facingRight){
            Flip(180);
        }
    }

    private IEnumerator Dash(){
        _isDashing = true;

        Vector3 dashDirection = _facingRight ? Vector3.forward : -Vector3.forward;
        _rb.AddForce(dashDirection * _dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(_dashDuration);

        _isDashing = false;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            _isGrounded = true;
        }
    }

    private void Flip(Int16 value){
        _facingRight = !_facingRight;

        Vector3 rotation = _objectToFlip.transform.eulerAngles;
        rotation.y = value;
        _objectToFlip.transform.eulerAngles = rotation;
    }
}