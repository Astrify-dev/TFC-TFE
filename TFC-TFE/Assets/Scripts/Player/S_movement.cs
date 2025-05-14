using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using System;
using System.Runtime.InteropServices;

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
    private float _rebounceBoostMultiplier = 1.1f;
    private float _reboundTimer = 1f;
    private float _reboundInputWindow = 0.5f;
    private float _minMoveSpeed;
    private float _maxMoveSpeed;
    private float _accelerationRate;
    private float _currentMoveSpeed;
    private float _minJumpForce;
    private float _maxJumpForce;

    [Header("Paramètres Raycast")]
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private float _wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _bounceLayer;


    [Header("Couleurs des Raycasts")]
    [SerializeField] private Color _groundRayColor = Color.green;
    [SerializeField] private Color _wallRayColor = Color.red;
    [SerializeField] private Color _ceilingRayColor = Color.blue;
    [SerializeField] private Color _bounceRayColor = Color.yellow;
    [SerializeField] private Color _velocityRayColor = Color.yellow;

    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _isGrounded = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _isDashing = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canJump = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canDash = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canAirDash = true;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _groundDashCooldown = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _airDashCooldown = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _canWallJump = false;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private float _wallJumpTimer = 0f;
    [SerializeField, BoxGroup("ReadOnly"), ReadOnly] private bool _hasAirDashed = false;

    private bool _facingRight = true;
    private bool _groundDashOnCooldown = false;
    private bool _waitingForReboundInput = false;
    private bool _isAirReboundDash = false;
    private bool _isWallSliding = false;

    private Rigidbody _rb;
    
    private Vector2 _moveInput;

    private Coroutine _slowMotionCoroutine;
    private Vector3 _vector3AxeProject = new Vector3(0, 1, 1);
    private void Start(){
        S_controllerPlayer.Instance.inputPlayer.OnMoveEvent += HandleMove;
        S_controllerPlayer.Instance.inputPlayer.OnJumpEvent += HandleJump;
        S_controllerPlayer.Instance.inputPlayer.OnDashEvent += HandleDash;
        S_controllerPlayer.Instance.inputPlayer.OnDashReleased += OnDashRelease;

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
        S_controllerPlayer.Instance.inputPlayer.OnDashReleased -= OnDashRelease;

    }

    private void FixedUpdate(){
        if (_isDashing) return;

        CheckGrounded();
        CheckWallCollision();

        float moveInput = _moveInput.x;

        if (_canWallJump && !_isGrounded && ((moveInput > 0 && _facingRight) || (moveInput < 0 && !_facingRight))){
            moveInput = 0;
        }

        float targetSpeed = moveInput * _maxMoveSpeed;
        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, targetSpeed, _accelerationRate * Time.fixedDeltaTime);

        Vector3 velocity = _rb.velocity;
        velocity.z = _currentMoveSpeed;
        _rb.velocity = velocity;

        if (moveInput > 0 && !_facingRight){
            Flip(0);
        }else if (moveInput < 0 && _facingRight){
            Flip(180);
        }

        if (_canWallJump && !_isGrounded){
            StartWallSlide();
        }else{
            StopWallSlide();
        }

        if (_isWallSliding){
            Vector3 velocityUpdate = _rb.velocity;
            velocityUpdate.y = -_wallSlideSpeed;
            _rb.velocity = velocityUpdate;
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
        _rebounceBoostMultiplier = _movementSettings.rebounceBoostMultiplier;
        _reboundTimer = _movementSettings.reboundTimer;
        _reboundInputWindow = _movementSettings.reboundInputWindow;
        _minMoveSpeed = _movementSettings.minMoveSpeed;
        _maxMoveSpeed = _movementSettings.maxMoveSpeed;
        _accelerationRate = _movementSettings.accelerationRate;
        _minJumpForce = _movementSettings.minJumpForce;
        _maxJumpForce = _movementSettings.maxJumpForce;
    }

    #region RAYCAST
    private void CheckGrounded(){
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);
        if (_isGrounded){
            _canJump = true;
            _canDash = true;
            _canAirDash = true;
            _wallJumpTimer = 0;
            _canWallJump = false;
            _hasAirDashed = false;
        }
    }


    private void CheckWallCollision(){
        Vector3 direction = _facingRight ? Vector3.forward : Vector3.back;
        if (Physics.Raycast(transform.position, direction, out _, _wallCheckDistance, _wallLayer)){
            _canWallJump = true;
            _wallJumpTimer = _wallJumpCoyoteTime;
        }else if (_wallJumpTimer > 0){
            _wallJumpTimer -= Time.deltaTime;
        }else{
            _canWallJump = false;
            _wallJumpTimer = 0;
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

    #region SLIDE
    private void StartWallSlide(){
        if (_isWallSliding) return;

        _isWallSliding = true;
        _rb.useGravity = false;
    }

    private void StopWallSlide()
    {
        if (!_isWallSliding) return;

        _isWallSliding = false;
        _rb.useGravity = true; // réactive la gravité
    }
    #endregion

    #region JUMP
    private void HandleJump(){
        if (_isGrounded && _canJump){
            float speedRatio = Mathf.InverseLerp(_minMoveSpeed, _maxMoveSpeed, Mathf.Abs(_currentMoveSpeed));
            float dynamicJumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, speedRatio);

            _rb.AddForce(Vector3.up * dynamicJumpForce, ForceMode.Impulse);
            _isGrounded = false;
            _canJump = false;

        }else if (_canWallJump || _wallJumpTimer > 0){
            _rb.velocity = Vector3.zero;
            _rb.AddForce(_vector3AxeProject * _wallJumpForce, ForceMode.Impulse);

            _canWallJump = false;
            _wallJumpTimer = 0;
        }
    }

    #endregion

    #region DASH
    private void HandleDash(){
        if (!_canDash || _isDashing || (_groundDashCooldown && _groundDashOnCooldown)) return;

        if (_isGrounded){
            PerformGroundDash();
        }else if (_canAirDash){
            StartSlowMotionDash();
        }
    }

    private void PerformGroundDash(){
        _isDashing = true;
        _canDash = false;

        _isAirReboundDash = false;

        Vector3 dashDirection = _facingRight ? transform.forward : -transform.forward;
        _rb.AddForce(dashDirection * _groundDashForce, ForceMode.Impulse);
        Invoke(nameof(ResetDash), 0.1f);

        if (_groundDashCooldown){
            _groundDashOnCooldown = true;
            StartCoroutine(DashCooldown(_groundDashCooldownTime));
        }
    }



    private void PerformAirDash(){
        _isDashing = true;
        _canAirDash = false;
        _hasAirDashed = true;
        _isAirReboundDash = true;

        Vector3 dashDirection = new Vector3(0, _moveInput.y, _moveInput.x).normalized;
        if (dashDirection == Vector3.zero){
            dashDirection = _facingRight ? Vector3.forward : Vector3.back;
        }

        StartCoroutine(HandleAirDashWithRebound(dashDirection, _airDashForce));
    }

    private IEnumerator HandleAirDashWithRebound(Vector3 direction, float dashForce){
        float rayLength = 1.2f;
        float reboundTimeout = 0f;
        bool waitingForReboundInput = false;

        _rb.useGravity = false;
        _isDashing = true;
        _rb.velocity = direction * dashForce;

        void ReboundInputHandler(){
            if (waitingForReboundInput){
                waitingForReboundInput = false;
            }
        }

        S_controllerPlayer.Instance.inputPlayer.OnDashEvent += ReboundInputHandler;

        while (true){
            Vector3 rayOrigin = transform.position + direction.normalized * 0.25f;

            if (_isAirReboundDash && Physics.Raycast(rayOrigin, direction, out RaycastHit hit, rayLength, _bounceLayer)){
                _rb.velocity = Vector3.zero;
                waitingForReboundInput = true;

                float inputWindow = _reboundInputWindow;
                float timer = 0f;

                while (_isAirReboundDash && waitingForReboundInput && timer < inputWindow){
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (waitingForReboundInput){
                    break;
                }

                Vector3 reflected = Vector3.Reflect(direction, hit.normal).normalized;

                if (Vector3.Angle(reflected, Vector3.down) < 20f)
                    reflected = new Vector3(reflected.x, -0.5f, reflected.z).normalized;

                direction = reflected;
                transform.position = hit.point + hit.normal * 0.05f;

                _rb.velocity = direction * dashForce * _rebounceBoostMultiplier;
                reboundTimeout = 0f;

                Debug.DrawRay(transform.position, direction * 2f, Color.magenta, 0.2f);
            }else{
                _rb.velocity = direction * dashForce;
            }

            if (direction.z > 0 && !_facingRight) Flip(0);
            else if (direction.z < 0 && _facingRight) Flip(180);

            reboundTimeout += Time.fixedDeltaTime;
            if (reboundTimeout >= _reboundTimer)
                break;

            yield return new WaitForFixedUpdate();
        }

        S_controllerPlayer.Instance.inputPlayer.OnDashEvent -= ReboundInputHandler;

        _rb.useGravity = true;
        _isDashing = false;
        _hasAirDashed = false;
        _isAirReboundDash = false;
    }


    private void OnDashRelease(){
        if (_canAirDash){
            S_controllerPlayer.Instance.slowMotionHandler.StopSlowMotion();
            if(!_groundDashOnCooldown)
                PerformAirDash();
        }
    }


    private void StartSlowMotionDash(){
        S_controllerPlayer.Instance.slowMotionHandler.StartSlowMotion(_intensiteSlow, _timerSlow, OnSlowMotionEnd);
    }


    private IEnumerator WaitForDashRelease(){
        while (S_controllerPlayer.Instance.inputPlayer._inputs.Player.Dash.IsPressed()){
            yield return null;
        }

        S_controllerPlayer.Instance.slowMotionHandler.StopSlowMotion();
        PerformAirDash();
    }

    private void OnSlowMotionEnd(){
        if (!S_controllerPlayer.Instance.inputPlayer._inputs.Player.Dash.IsPressed())
            return;

        PerformAirDash();
    }

    private IEnumerator DashCooldown(float cooldownTime){
        yield return new WaitForSeconds(cooldownTime);
        _groundDashOnCooldown = false;
        _canDash = true;
    }



    private void ResetDash(){
        _isDashing = false;
    }
    #endregion

    #region GIZMOS
    private void OnDrawGizmos(){
        if (_rb == null) return;

        // Raycast pour vérifier le sol
        Gizmos.color = _groundRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);

        // Raycast pour vérifier les murs
        Gizmos.color = _wallRayColor;
        Vector3 wallDirection = _facingRight ? Vector3.forward : Vector3.back;
        Gizmos.DrawLine(transform.position, transform.position + wallDirection * _wallCheckDistance);

        // Raycast pour vérifier le plafond
        Gizmos.color = _ceilingRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _groundCheckDistance);

        // Gizmo pour la direction du Dash (basé sur l'entrée du joystick)
        Gizmos.color = _bounceRayColor;
        Vector3 dashDirection = new Vector3(0, _moveInput.y, _moveInput.x).normalized; // Haut/Bas = Y, Gauche/Droite = Z
        Gizmos.DrawLine(transform.position, transform.position + dashDirection * 5); // Longueur arbitraire de 5 unités

        // Gizmo pour la direction de la vélocité actuelle
        Gizmos.color = _velocityRayColor;
        Vector3 velocityDirection = _rb.velocity.normalized; // Direction de la vélocité
        Gizmos.DrawLine(transform.position, transform.position + velocityDirection * 5); // Longueur arbitraire de 5 unités

        // GIZMO : Raycast de détection de rebond
        if (_isDashing){
            Gizmos.color = Color.magenta;
            Vector3 raycastDir = _rb.velocity.normalized;
            Vector3 rayOrigin = transform.position + raycastDir * 0.25f;
            Gizmos.DrawLine(rayOrigin, rayOrigin + raycastDir * 1.5f);
        }
    }
    #endregion
}
