using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using System;

public class S_movement : MonoBehaviour{
    [Header("Paramètres de déplacement")]
    [SerializeField] private PlayerMovementSettings _movementSettings;
    [SerializeField] private GameObject _objectToFlip;

    private float _moveSpeed = 5f;
    private float _dive = 20f;
    private float _jumpForce = 5f;
    private float _dashForce = 10f;
    private float _dashCooldownTime = 1f;
    private float _intensiteSlow = 0.5f;
    private float _timerSlow = 10f;
    

    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _isGrounded = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _isDashing = false;
    private bool _facingRight = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canJump = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canDash = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _dashCooldown = false;

    private Rigidbody _rb;
    
    private Vector2 _moveInput;

    private Coroutine _slowMotionCoroutine;

    private void Awake(){
        if (_rb is null){
            _rb = GetComponent<Rigidbody>();
        }
        if (_objectToFlip is null){
            _objectToFlip = gameObject;
        }
        ApplyMovementSettings();
    }

    private void OnEnable(){
        S_controllerPlayer.Instance.inputPlayer.OnMoveEvent += HandleMove;
        S_controllerPlayer.Instance.inputPlayer.OnJumpEvent += HandleJump;
        S_controllerPlayer.Instance.inputPlayer.OnDashEvent += HandleDash;
    }

    private void OnDisable(){
        S_controllerPlayer.Instance.inputPlayer.OnMoveEvent -= HandleMove;
        S_controllerPlayer.Instance.inputPlayer.OnJumpEvent -= HandleJump;
        S_controllerPlayer.Instance.inputPlayer.OnDashEvent -= HandleDash;
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
        PerformDive();
    }

    private void OnCollisionEnter(Collision collision){
        if (_movementSettings is not null && (_movementSettings.jumpResetLayers.value & (1 << collision.gameObject.layer)) != 0){
            _isGrounded = true;
            _canJump = true;
        }

        if (_movementSettings is not null && (_movementSettings.dashResetLayers.value & (1 << collision.gameObject.layer)) != 0){
            _isDashing = false;
            _canDash = true;
        }
    }

    private void ApplyMovementSettings(){
        if (_movementSettings is null) return;
        
        _moveSpeed = _movementSettings.moveSpeed;
        _dive = _movementSettings.dive;
        _jumpForce = _movementSettings.jumpForce;
        _dashForce = _movementSettings.dashForce;
        _dashCooldownTime = _movementSettings.dashCooldownTime;
        _dashCooldown = _movementSettings.dashCooldown;
        _intensiteSlow = _movementSettings.slowMotionIntensity;
        _timerSlow = _movementSettings.slowMotionTimer;
        _movementSettings.ApplySettingsToRigidbody(_rb);
    }

    #region MOVE
    private void HandleMove(Vector2 moveInput){
        _moveInput = Vector2.MoveTowards(_moveInput, moveInput, Time.deltaTime * (_moveSpeed * 100));
    }

    private void Flip(short value){
        _facingRight = !_facingRight;

        Vector3 rotation = _objectToFlip.transform.eulerAngles;
        rotation.y = value;
        _objectToFlip.transform.eulerAngles = rotation;
    }

    private void PerformDive(){
        if (!_isGrounded && _moveInput.y < 0){
            Vector3 diveForce = new Vector3(0, _moveInput.y, 0) * _dive;
            _rb.AddForce(diveForce, ForceMode.Acceleration);
        }
    }

    #endregion

    #region JUMP
    private void HandleJump(){
        if (_isGrounded && _canJump){
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
            _canJump = false;
        }
    }
    #endregion

    #region DASH
    private void HandleDash(){
        if (!_canDash || _isDashing) return;

        if (_isGrounded){
            PerformDash();
        }else{
            StartSlowMotionDash();
        }
    }

    private void PerformDash(){
        _isDashing = true;
        _canDash = false;

        Vector3 dashDirection = new Vector3(0, _moveInput.y, _moveInput.x).normalized;
        _rb.AddForce(dashDirection * _dashForce, ForceMode.Impulse);

        Invoke(nameof(ResetDash), 0.1f);

        if (_dashCooldown){
            StartCoroutine(DashCooldown());
        }else{
            _canDash = true;
        }
    }

    private void StartSlowMotionDash(){
        S_controllerPlayer.Instance.slowMotionHandler.StartSlowMotion(
            _intensiteSlow,
            _timerSlow,
            OnSlowMotionEnd
        );
        StartCoroutine(WaitForDashRelease());
    }

    private IEnumerator WaitForDashRelease()
    {
        while (S_controllerPlayer.Instance.inputPlayer._inputs.Player.Dash.IsPressed())
        {
            yield return null;
        }

        S_controllerPlayer.Instance.slowMotionHandler.StopSlowMotion();
        PerformDash();
    }

    private void OnSlowMotionEnd()
    {
        // Si le slow motion se termine avant que le joueur ne relâche le bouton, on ne dash pas.
        if (!S_controllerPlayer.Instance.inputPlayer._inputs.Player.Dash.IsPressed())
        {
            return;
        }

        PerformDash();
    }
    private IEnumerator DashCooldown(){
        yield return new WaitForSeconds(_dashCooldownTime);
        _canDash = true;
    }

    private void ResetDash(){
        _isDashing = false;
    }
    #endregion
}