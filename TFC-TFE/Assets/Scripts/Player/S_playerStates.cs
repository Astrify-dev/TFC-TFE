using System.Collections;
using UnityEngine;

public class S_playerStates : MonoBehaviour
{
    private S_basePlayerStates _currentState;

    [Header("Références")]
    [SerializeField] private GameObject _objectToFlip;
    [SerializeField] private PlayerMovementSettings _movementSettings;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Détection environnement")]
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private float _wallCheckDistance = 0.5f;

    [Header("Couleurs des Raycasts")]
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

    private bool awaitingReboundInput = false;

    public PlayerMovementSettings Settings => _movementSettings;
    public S_SlowMotion SlowMotion { get; private set; }

    // États
    public S_initialyzePlayerState _initialyzePlayerState { get; private set; } = new S_initialyzePlayerState();
    public S_groundPlayerState GroundState { get; private set; } = new S_groundPlayerState();
    public S_airPlayerState AirState { get; private set; } = new S_airPlayerState();
    public S_JumpPlayerState JumpState { get; private set; } = new S_JumpPlayerState();
    public S_airDashPlayerState AirDashState { get; private set; } = new S_airDashPlayerState();
    public S_dashGroundPlayerState DashGroundState { get; private set; } = new S_dashGroundPlayerState();
    public S_slideWallPlayerState SlideWallState { get; private set; } = new S_slideWallPlayerState();
    public S_wallDashReboundPlayerState WallReboundState { get; private set; } = new S_wallDashReboundPlayerState();
    public S_deadPlayerState DeadState { get; private set; } = new S_deadPlayerState();

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
                SwitchState(JumpState);
        };

        input.OnDashEvent += () =>
        {
            if (awaitingReboundInput)
                return;

            if (_currentState == GroundState && CanDash)
                SwitchState(DashGroundState);
            else if ((_currentState == AirState || _currentState == SlideWallState) && CanAirDash)
                StartSlowMotionDash();
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

        // Limite la vitesse de chute à une valeur max pour éviter un freefall incontrôlé
        Vector3 vel = Rigidbody.velocity;
        if (vel.y < -_maxFallSpeed)
            vel.y = -_maxFallSpeed;

        Rigidbody.velocity = vel;
    }

    private void OnEnable() => _currentState?.OnEnable(this);
    private void OnDisable() => _currentState?.OnDisable(this);

    public void SwitchState(S_basePlayerStates newState)
    {
        _currentState?.OnDisable(this);
        _currentState = newState;
        Debug.Log($"<color=orange>[STATE]</color> Switched to: {newState.GetType().Name}");
        newState?.OnEnable(this);
        newState?.EnterState(this);
    }

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
        // Raycast ou SphereCast pour détecter le sol (jump reset possible)
        if (Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, Settings.jumpResetLayers))
            return true;

        return Physics.SphereCast(transform.position, 0.25f, Vector3.down, out _, _groundCheckDistance + 0.2f, Settings.jumpResetLayers);
    }

    public bool CheckWall()
    {
        Vector3 dir = FacingRight ? Vector3.forward : Vector3.back;
        Ray ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hit, _wallCheckDistance, Settings.wallJumpLayers))
        {
            Debug.DrawRay(ray.origin, ray.direction * _wallCheckDistance, Color.green);
            Debug.Log($"<color=green>[WALL CHECK]</color> Hit {hit.collider.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)} ✅");
            return true;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * _wallCheckDistance, Color.red);
            Debug.Log($"<color=red>[WALL CHECK]</color> Nothing hit ❌");
            return false;
        }
    }

    public void StartSlowMotionDash() =>
        SlowMotion?.StartSlowMotion(Settings.slowMotionIntensity, Settings.slowMotionTimer);

    public void StopSlowMotionDashAndPerformAirDash()
    {
        SlowMotion?.StopSlowMotion();
        SwitchState(AirDashState);
    }

    public void StartAirDashCoroutine(Vector3 direction)
    {
        StartCoroutine(HandleAirDashWithRebound(direction, Settings.airDashForce));
    }

    private IEnumerator HandleAirDashWithRebound(Vector3 direction, float dashForce)
    {
        float rayLength = 1.2f;
        float reboundTimeout = 0f;

        Rigidbody.useGravity = false;
        IsDashing = true;

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

                if (awaitingReboundInput)
                    break;

                Vector3 reflected = Vector3.Reflect(direction, hit.normal).normalized;

                if (Vector3.Angle(reflected, Vector3.down) < 20f)
                    reflected = new Vector3(reflected.x, -0.5f, reflected.z).normalized;

                direction = reflected;
                transform.position = hit.point + hit.normal * 0.05f;

                Rigidbody.velocity = direction * dashForce * Settings.rebounceBoostMultiplier;

                HandleFlip(direction.x);

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
        IsAirReboundDash = false;

        SwitchState(AirState);
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
}
