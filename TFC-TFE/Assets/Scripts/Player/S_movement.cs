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
    private float _wallJumpForce = 1.2f;
    private float _wallJumpCoyoteTime = 0.2f;
    private float _wallSlideSpeed = 2f;
    private float _groundDashForce = 10f;
    private float _airDashForce = 7f; 
    private float _groundDashCooldownTime = 1f;
    private float _intensiteSlow = 0.5f;
    private float _timerSlow = 10f;

    [Header("Paramètres Raycast")]
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private float _wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Couleurs des Raycasts")]
    [SerializeField] private Color _groundRayColor = Color.green;
    [SerializeField] private Color _wallRayColor = Color.red;

    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _isGrounded = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _isDashing = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canJump = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canDash = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canAirDash = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _groundDashCooldown = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _airDashCooldown = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canWallJump = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private float _wallJumpTimer = 0f;

    private bool _facingRight = true;

    private Rigidbody _rb;
    
    private Vector2 _moveInput;

    private Coroutine _slowMotionCoroutine;

    private void Awake(){

    }

    private void Start(){
        S_controllerPlayer.Instance.inputPlayer.OnMoveEvent += HandleMove;
        S_controllerPlayer.Instance.inputPlayer.OnJumpEvent += HandleJump;
        S_controllerPlayer.Instance.inputPlayer.OnDashEvent += HandleDash;
        if (_rb is null){
            _rb = GetComponent<Rigidbody>();
        }

        if (_objectToFlip is null){
            _objectToFlip = gameObject;
        }

        ApplyMovementSettings();
    }

    private void OnDisable(){
        S_controllerPlayer.Instance.inputPlayer.OnMoveEvent -= HandleMove;
        S_controllerPlayer.Instance.inputPlayer.OnJumpEvent -= HandleJump;
        S_controllerPlayer.Instance.inputPlayer.OnDashEvent -= HandleDash;
    }

    private void FixedUpdate(){
        if (_isDashing) return;

        CheckGrounded();
        CheckWallCollision();

        float moveInput = _moveInput.x;

        if (_canWallJump && !_isGrounded && ((moveInput > 0 && _facingRight) || (moveInput < 0 && !_facingRight))){
            moveInput = 0;
        }
        Vector3 move = new Vector3(0, 0, moveInput * _moveSpeed);
        _rb.velocity = new Vector3(0, _rb.velocity.y, move.z);

        if (moveInput > 0 && !_facingRight){
            Flip(0);
        }else if (moveInput < 0 && _facingRight){
            Flip(180);
        }

        if (_canWallJump && !_isGrounded && _rb.velocity.y > -_wallSlideSpeed){
            _rb.velocity = new Vector3(_rb.velocity.x, -_wallSlideSpeed, _rb.velocity.z);
        }

        PerformDive();
    }

    private void ApplyMovementSettings(){
        if (_movementSettings is null) return;
        
        _moveSpeed = _movementSettings.moveSpeed;
        _dive = _movementSettings.dive;
        _jumpForce = _movementSettings.jumpForce;
        _wallJumpCoyoteTime = _movementSettings.wallJumpCoyoteTime;
        _wallJumpForce = _movementSettings.wallJumpForce;
        _groundDashForce = _movementSettings.groundDashForce;
        _groundDashCooldownTime = _movementSettings.groundDashCooldownTime;
        _airDashForce = _movementSettings.airDashForce;
        _intensiteSlow = _movementSettings.slowMotionIntensity;
        _timerSlow = _movementSettings.slowMotionTimer;
        _wallSlideSpeed = _movementSettings.wallSlideSpeed;
        _groundDashCooldown = _movementSettings.groundDashCooldown;
        _movementSettings.ApplySettingsToRigidbody(_rb);
    }

    #region RAYCAST
    private void CheckGrounded(){
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance, _groundLayer);
        if (_isGrounded){
            _canJump = true;
            _canDash = true;
            _canAirDash = true;
            _wallJumpTimer = 0;
            _canWallJump = false;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _groundCheckDistance, _movementSettings.dashResetLayers)){
            _canAirDash = true;
        }
    }

    private void CheckWallCollision(){
        Vector3 direction = _facingRight ? Vector3.forward : Vector3.back;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, _wallCheckDistance, _wallLayer)){
            _canWallJump = true;
            _wallJumpTimer = _wallJumpCoyoteTime;
        }else if (_wallJumpTimer > 0){
            _wallJumpTimer -= Time.deltaTime;
        }else{
            _canWallJump = false;
            _wallJumpTimer = 0;

        }
        if (Physics.Raycast(transform.position, direction, out hit, _wallCheckDistance, _movementSettings.dashResetLayers)){
            _canAirDash = true;
        }
    }
    #endregion

    #region MOVE
    private void HandleMove(Vector2 moveInput){
        _moveInput = Vector2.MoveTowards(_moveInput, moveInput, Time.deltaTime * (_moveSpeed * 100));
    }

    private void Flip(int value){
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

        }else if (_canWallJump || _wallJumpTimer > 0){
            _rb.velocity = Vector3.zero;
            _rb.AddForce(Vector3.up * _wallJumpForce, ForceMode.Impulse);

            _canWallJump = false;
            _wallJumpTimer = 0;
        }
    }


    #endregion

    #region DASH
    private void HandleDash(){
        if (!_canDash || _isDashing) return;

        if (_isGrounded){
            PerformDash();
        }else if (_canAirDash){ 
            StartSlowMotionDash();
            _canAirDash = false;
        }
    }

    private void PerformDash(){
        _isDashing = true;
        _canDash = false;

        float dashForce = _isGrounded ? _groundDashForce : _airDashForce;
        Vector3 dashDirection;
        if (_moveInput == Vector2.zero){
            dashDirection = _facingRight ? transform.forward : -transform.forward;
        }else{
            dashDirection = new Vector3(0, _moveInput.y, _moveInput.x).normalized;
        }

        _rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        Invoke(nameof(ResetDash), 0.1f);

        if (_isGrounded && _groundDashCooldown){
            StartCoroutine(DashCooldown(_groundDashCooldownTime));
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

    private IEnumerator WaitForDashRelease(){
        while (S_controllerPlayer.Instance.inputPlayer._inputs.Player.Dash.IsPressed()){
            yield return null;
        }

        S_controllerPlayer.Instance.slowMotionHandler.StopSlowMotion();
        PerformDash();
    }

    private void OnSlowMotionEnd(){
        if (!S_controllerPlayer.Instance.inputPlayer._inputs.Player.Dash.IsPressed()){
            return;
        }

        PerformDash();
    }
    private IEnumerator DashCooldown(float cooldownTime){
        yield return new WaitForSeconds(cooldownTime);
        _canDash = true;
    }


    private void ResetDash(){
        _isDashing = false;
    }
    #endregion

    #region GIZMOS
    private void OnDrawGizmos(){
        Gizmos.color = _groundRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);

        Gizmos.color = _wallRayColor;
        Vector3 direction = _facingRight ? Vector3.forward : Vector3.back;
        Gizmos.DrawLine(transform.position, transform.position + direction * _wallCheckDistance);

    }
    #endregion
}