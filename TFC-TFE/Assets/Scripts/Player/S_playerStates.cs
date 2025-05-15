using System.Collections;
using UnityEngine;

public class S_playerStates : MonoBehaviour
{
    #region === Références & Données ===

    private S_basePlayerStates _currentState;

    [Header("Références")]
    [SerializeField] private GameObject _objectToFlip;
    [SerializeField] private PlayerMovementSettings _movementSettings;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Détection environnement")]
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private float _wallCheckDistance = 0.5f;

    [Header("Debug Raycast Couleurs")]
    [SerializeField] private Color _groundRayColor = Color.green;
    [SerializeField] private Color _wallRayColor = Color.red;
    [SerializeField] private Color _ceilingRayColor = Color.blue;
    [SerializeField] private Color _bounceRayColor = Color.yellow;
    [SerializeField] private Color _velocityRayColor = Color.yellow;

    [SerializeField] private float _maxFallSpeed = 25f;

    public Rigidbody Rigidbody => _rigidbody;
    public Vector2 MoveInput { get; set; }
    public bool FacingRight { get; private set; } = true;

    public bool IsGrounded { get; set; }
    public bool CanJump { get; set; }
    public bool CanDash { get; set; }
    public bool CanAirDash { get; set; }
    public bool IsDashing { get; set; }
    public bool CanWallJump { get; set; }
    public float WallJumpTimer { get; set; }
    public bool HasAirDashed { get; set; }
    public bool IsWallSliding { get; set; }
    public bool IsAirReboundDash { get; set; }

    public bool WasReboundDash { get; private set; } = false;
    public bool HasActuallyRebounded { get; private set; } = false;

    private bool awaitingReboundInput = false;
    private float _chargedJumpForce = 0f;
    private Coroutine _jumpChargeCoroutine;
    private Coroutine _airDashCoroutine;

    public float AirControlLockTimer { get; set; } = 0f;
    public float MovementLockTimer { get; set; } = 0f;

    public PlayerMovementSettings Settings => _movementSettings;
    public S_SlowMotion SlowMotion { get; private set; }

    #endregion

    #region === États de la State Machine ===

    public S_initialyzePlayerState _initialyzePlayerState { get; private set; } = new S_initialyzePlayerState();
    public S_groundPlayerState GroundState { get; private set; } = new S_groundPlayerState();
    public S_airPlayerState AirState { get; private set; } = new S_airPlayerState();
    public S_JumpPlayerState JumpState { get; private set; } = new S_JumpPlayerState();
    public S_airDashPlayerState AirDashState { get; private set; } = new S_airDashPlayerState();
    public S_dashGroundPlayerState DashGroundState { get; private set; } = new S_dashGroundPlayerState();
    public S_slideWallPlayerState SlideWallState { get; private set; } = new S_slideWallPlayerState();
    public S_wallDashReboundPlayerState WallReboundState { get; private set; } = new S_wallDashReboundPlayerState();
    public S_deadPlayerState DeadState { get; private set; } = new S_deadPlayerState();

    #endregion

    #region === Unity Lifecycle ===

    private void Awake()
    {
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if (_objectToFlip == null) _objectToFlip = gameObject;
    }

    private void Start()
    {
        _movementSettings.ApplySettingsToRigidbody(_rigidbody);
        SlowMotion = S_controllerPlayer.Instance.slowMotionHandler;

        var input = S_controllerPlayer.Instance.inputPlayer;

        input.OnMoveEvent += inputVector => MoveInput = inputVector;

        input.OnJumpEvent += () =>
        {
            if (_currentState == GroundState || _currentState == SlideWallState)
            {
                if (_jumpChargeCoroutine != null)
                    StopCoroutine(_jumpChargeCoroutine);

                _jumpChargeCoroutine = StartCoroutine(ChargeJump());
            }
        };

        input.OnJumpReleased += () =>
        {
            if (_currentState == GroundState)
            {
                PerformChargedJump();
            }
            else if (_currentState == SlideWallState && CanWallJump)
            {
                PerformWallJump();
            }
            else if (IsDashing)
            {
                BreakDashWithJump();
            }
        };

        input.OnDashEvent += () =>
        {
            if (awaitingReboundInput) return;

            if ((_currentState == AirState || _currentState == SlideWallState) && CanAirDash)
            {
                if (!WasReboundDash)
                {
                    Debug.Log("<color=red>[DASH]</color> Interdit : dash après saut non-rebond");
                    return;
                }

                Debug.Log("<color=green>[DASH]</color> Autorisé : dash après rebond");
                StartSlowMotionDash();
            }
            else if (_currentState == GroundState && CanDash)
            {
                SwitchState(DashGroundState);
            }
        };

        input.OnDashReleased += () =>
        {
            if ((_currentState == AirState || _currentState == SlideWallState) && CanAirDash)
                StopSlowMotionDashAndPerformAirDash();
        };

        SwitchState(_initialyzePlayerState);
    }

    private void Update()
    {
        _currentState?.UpdateState(this);

        if (AirControlLockTimer > 0f)
            AirControlLockTimer -= Time.deltaTime;

        Vector3 vel = Rigidbody.velocity;
        if (vel.y < -_maxFallSpeed)
            vel.y = -_maxFallSpeed;
        Rigidbody.velocity = vel;

        if (IsDashing && CheckWall())
        {
            Vector3 correctionDir = FacingRight ? Vector3.forward : Vector3.back;
            Ray wallRay = new Ray(transform.position, correctionDir);

            if (Physics.Raycast(wallRay, out RaycastHit hit, _wallCheckDistance + 0.15f, Settings.wallJumpLayers))
            {
                Debug.DrawLine(transform.position, hit.point, Color.magenta);
                Vector3 targetPos = hit.point - correctionDir * 0.5f;
                transform.position = new Vector3(transform.position.x, transform.position.y, targetPos.z);
            }
        }
    }

    private void OnEnable() => _currentState?.OnEnable(this);
    private void OnDisable() => _currentState?.OnDisable(this);

    #endregion

    #region === Transitions d'État ===

    public void SwitchState(S_basePlayerStates newState)
    {
        _currentState?.OnDisable(this);
        _currentState = newState;
        newState?.OnEnable(this);
        newState?.EnterState(this);
    }

    #endregion

    #region === Mouvement, Dash & Jump ===

    public void StartSlowMotionDash() =>
        SlowMotion?.StartSlowMotion(Settings.slowMotionIntensity, Settings.slowMotionTimer);

    public void StopSlowMotionDashAndPerformAirDash()
    {
        SlowMotion?.StopSlowMotion();
        SwitchState(AirDashState);
    }

    public void PerformWallJump()
    {
        Rigidbody.velocity = Vector3.zero;
        int direction = FacingRight ? -1 : 1;

        Vector2 impulse = new Vector2(direction * Settings.directionImpulsion.x, Settings.directionImpulsion.y);
        Vector3 wallJumpForce = new Vector3(0, impulse.y, impulse.x) * Settings.wallJumpForce;

        Rigidbody.AddForce(wallJumpForce, ForceMode.Impulse);
        HandleFlip(direction);

        CanWallJump = false;
        WallJumpTimer = 0f;
        AirControlLockTimer = 0.3f;
        CanAirDash = true;

        MoveInput = Vector2.zero;

        S_controllerPlayer.Instance.inputPlayer.EnableMove(false);
        StartCoroutine(ReactivateInputAfterDelay(0.25f));

        SwitchState(AirState);
    }

    public void BreakDashWithJump()
    {
        if (!IsDashing) return;

        Vector3 currentVelocity = Rigidbody.velocity;
        Vector3 blendedVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 5f);
        Rigidbody.velocity = blendedVelocity;

        int direction = FacingRight ? 1 : -1;
        Vector2 impulse2D = new Vector2(direction * Settings.dashJumpDirection.x, Settings.dashJumpDirection.y);
        Vector3 jumpForce = new Vector3(0f, impulse2D.y, impulse2D.x) * Settings.dashJumpForce;

        Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);

        MovementLockTimer = 0.1f;
        AirControlLockTimer = 0.3f;

        HandleFlip(direction);

        if (_airDashCoroutine != null)
        {
            StopCoroutine(_airDashCoroutine);
            _airDashCoroutine = null;
        }

        IsDashing = false;
        IsAirReboundDash = false;
        MoveInput = Vector2.zero;

        CanAirDash = HasActuallyRebounded;

        if (CanAirDash)
            Debug.Log("<color=green>[DASH]</color> Autorisé : dash après rebond réel");
        else
            Debug.Log("<color=red>[DASH]</color> Interdit : dash après saut sans rebond");

        SwitchState(AirState);
    }

    public void PerformChargedJump()
    {
        if (_chargedJumpForce > 0f)
        {
            Rigidbody.AddForce(Vector3.up * _chargedJumpForce, ForceMode.Impulse);
            IsGrounded = false;
            CanJump = false;
            _chargedJumpForce = 0f;
            SwitchState(AirState);
        }
    }

    public void StartVariableJump() => StartCoroutine(ChargeJump());

    private IEnumerator ChargeJump()
    {
        float timer = 0f;
        float maxHoldTime = Settings.jumpChargeTime;
        _chargedJumpForce = Settings.minJumpForce;

        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

        while (timer < maxHoldTime)
        {
            if (!S_controllerPlayer.Instance.inputPlayer.IsJumpHeld)
                break;

            timer += Time.deltaTime;
            float ratio = Mathf.Clamp01(timer / maxHoldTime);
            _chargedJumpForce = Mathf.Lerp(Settings.minJumpForce, Settings.maxJumpForce, ratio);

            yield return null;
        }
    }

    private IEnumerator ReactivateInputAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        S_controllerPlayer.Instance.inputPlayer.EnableMove(true);
    }

    #endregion

    #region === Rebond Dash ===

    public void StartAirDashCoroutine(Vector3 direction)
    {
        WasReboundDash = false;
        HasActuallyRebounded = false;
        _airDashCoroutine = StartCoroutine(HandleAirDashWithRebound(direction, Settings.airDashForce));
    }

    private IEnumerator HandleAirDashWithRebound(Vector3 direction, float dashForce)
    {
        float rayLength = 1.2f;
        float reboundTimeout = 0f;

        Rigidbody.useGravity = false;
        IsDashing = true;
        WasReboundDash = IsAirReboundDash;
        Rigidbody.velocity = direction * dashForce;
        awaitingReboundInput = false;

        void ReboundInputHandler()
        {
            if (awaitingReboundInput)
                awaitingReboundInput = false;
        }

        S_controllerPlayer.Instance.inputPlayer.OnDashEvent += ReboundInputHandler;

        while (true)
        {
            Vector3 rayOrigin = transform.position + direction.normalized * 0.25f;

            if (IsAirReboundDash && Physics.Raycast(rayOrigin, direction, out RaycastHit hit, rayLength, Settings.bounceLayers))
            {
                Rigidbody.velocity = Vector3.zero;
                awaitingReboundInput = true;

                float timer = 0f;
                while (awaitingReboundInput && timer < Settings.reboundInputWindow)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (awaitingReboundInput) break;

                HasActuallyRebounded = true;

                Vector3 reflected = Vector3.Reflect(direction, hit.normal).normalized;
                if (Vector3.Angle(reflected, Vector3.down) < 20f)
                    reflected = new Vector3(reflected.x, -0.5f, reflected.z).normalized;

                direction = reflected;
                transform.position = hit.point + hit.normal * 0.05f;
                Rigidbody.velocity = direction * dashForce * Settings.rebounceBoostMultiplier;

                int directionVisuel = FacingRight ? -1 : 1;
                HandleFlip(directionVisuel);

                reboundTimeout = 0f;
            }
            else
            {
                Rigidbody.velocity = direction * dashForce;
            }

            reboundTimeout += Time.fixedDeltaTime;
            if (reboundTimeout >= Settings.reboundTimer)
                break;

            yield return new WaitForFixedUpdate();
        }

        S_controllerPlayer.Instance.inputPlayer.OnDashEvent -= ReboundInputHandler;

        Rigidbody.useGravity = true;
        IsDashing = false;
        HasAirDashed = false;

        SwitchState(AirState);
    }

    #endregion

    #region === Utilitaires ===

    public void HandleFlip(float moveInput)
    {
        if (moveInput > 0 && !FacingRight) SetFlip(0);
        else if (moveInput < 0 && FacingRight) SetFlip(180);
    }

    public void SetFlip(int value)
    {
        FacingRight = !FacingRight;
        Vector3 rot = _objectToFlip.transform.eulerAngles;
        rot.y = value;
        _objectToFlip.transform.eulerAngles = rot;
    }

    public bool CheckGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, Settings.jumpResetLayers))
            return true;

        return Physics.SphereCast(transform.position, 0.25f, Vector3.down, out _, _groundCheckDistance + 0.2f, Settings.jumpResetLayers);
    }

    public bool CheckWall()
    {
        float distance = IsDashing ? _wallCheckDistance + 0.3f : _wallCheckDistance;
        Vector3 dir = FacingRight ? Vector3.forward : Vector3.back;
        Ray ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hit, distance, Settings.wallJumpLayers))
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (_rigidbody == null) return;

        Gizmos.color = _groundRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);

        Gizmos.color = _wallRayColor;
        Vector3 wallDirection = FacingRight ? Vector3.forward : Vector3.back;
        Gizmos.DrawLine(transform.position, transform.position + wallDirection * _wallCheckDistance);

        Gizmos.color = _ceilingRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _groundCheckDistance);

        Gizmos.color = _bounceRayColor;
        Vector3 dashDirection = new Vector3(0, MoveInput.y, MoveInput.x).normalized;
        Gizmos.DrawLine(transform.position, transform.position + dashDirection * 5f);

        Gizmos.color = _velocityRayColor;
        Vector3 velocityDir = _rigidbody.velocity.normalized;
        Gizmos.DrawLine(transform.position, transform.position + velocityDir * 5f);

        if (IsDashing)
        {
            Gizmos.color = Color.magenta;
            Vector3 raycastDir = _rigidbody.velocity.normalized;
            Vector3 rayOrigin = transform.position + raycastDir * 0.25f;
            Gizmos.DrawLine(rayOrigin, rayOrigin + raycastDir * 1.5f);
        }
    }

    #endregion
}
