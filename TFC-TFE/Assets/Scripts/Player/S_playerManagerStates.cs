using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.VFX;

public class S_playerManagerStates : MonoBehaviour
{

    #region === Références & Données ===

    [Header("Références")]
    [SerializeField] GameObject _visualObject;
    [SerializeField] VisualEffect _visualEffectSphereObject;
    [SerializeField] public VisualEffect _hairEffect;
    [field: SerializeField] public LayerMask _invisibleWall { get; private set; }

    public ParticleSystem _groundDashParticles;
    [field: SerializeField] public PlayerMovementSettings MovementSettings { get; private set; }
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public TextMeshProUGUI SpeedShow { get; private set; }

    [Header("Détection environnement")]
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [field: SerializeField] public float WallCheckDistance { get; private set; } = 0.5f;

    [Header("Debug Raycast Couleurs")]
    [SerializeField] private Color _groundRayColor = Color.green;
    [SerializeField] private Color _wallRayColor = Color.red;
    [SerializeField] private Color _ceilingRayColor = Color.blue;
    [SerializeField] private Color _bounceRayColor = Color.yellow;
    [SerializeField] private Color _velocityRayColor = Color.yellow;

    [SerializeField] bool _debugState = false;

    [Header("Sound Design")]
    [SerializeField] public SoundSystem SFX_Dash;
    [SerializeField] public SoundSystem SFX_WallJump;
    [SerializeField] public SoundSystem SFX_Jump;
    [SerializeField] public SoundSystem SFX_Rebound;
    [SerializeField] public SoundSystem SFX_SlowMotion;
    [SerializeField] public SoundSystem SFX_Death;

    public bool IsDashing { get; private set; } = false;
    public Vector2 DirectionInput { get; private set; }
    public bool FacingRight { get; private set; }
    public bool EnableGroundDash { get; private set; } = true;
    public bool EnableWallSlide { get; private set; } = true;
    public bool PressRebounds { get; private set; } = true;
    public S_SlowMotion SlowMotion { get; private set; }

    public Animator AnimatorPlayer { get; private set; }
    public S_inputPlayer Inputs { get; private set; }
    public int AirDashCount { get; private set; }
    public Vector3 DashDirection { get; set; }
    public S_basePlayerStates CurrentState { get; private set; }

    private IEnumerator _reloadGroundDashCoroutine;
    private IEnumerator _reloadWallSlideCoroutine;
    private IEnumerator _durationPressReboundsCoroutine;
    private IEnumerator _disableSphereEffect;
    private Coroutine _currentTransition;

    private bool _airColorEnable;

    #endregion

    #region === États de la State Machine ===
    public S_initialyzePlayerState InitialyzePlayerState { get; private set; } = new S_initialyzePlayerState();
    public S_groundPlayerState GroundState { get; private set; } = new S_groundPlayerState();
    public S_airPlayerState AirState { get; private set; } = new S_airPlayerState();
    public S_JumpPlayerState JumpState { get; private set; } = new S_JumpPlayerState();
    public S_airDashPlayerState AirDashState { get; private set; } = new S_airDashPlayerState();
    public S_dashGroundPlayerState DashGroundState { get; private set; } = new S_dashGroundPlayerState();
    public S_slideWallPlayerState SlideWallState { get; private set; } = new S_slideWallPlayerState();
    public S_wallDashReboundPlayerState WallReboundState { get; private set; } = new S_wallDashReboundPlayerState();
    public S_deadPlayerState DeadState { get; private set; } = new S_deadPlayerState();
    public S_wallJumpPlayerState WallJumpState { get; private set; } = new S_wallJumpPlayerState();
    public S_slowMotionDashPlayerState SlowMotionDashState { get; private set; } = new S_slowMotionDashPlayerState();
    public S_AirJumpPlayerState AirJumpState { get; private set; } = new S_AirJumpPlayerState();
    #endregion

    private void Awake()
    {
        if (Rigidbody == null) Rigidbody = GetComponent<Rigidbody>();
        if (_visualObject == null) _visualObject = gameObject;

        Inputs = S_controllerPlayer.Instance.inputPlayer;
    }
    private void Start()
    {
        
        SlowMotion = S_controllerPlayer.Instance.slowMotionHandler;
        MovementSettings.ApplySettingsToRigidbody(Rigidbody);

        AnimatorPlayer = S_controllerPlayer.Instance.AnimatorPlayer;
        SwitchState(InitialyzePlayerState);

        _visualEffectSphereObject.gameObject.SetActive(false);   
        IsDashing = false;
    }

    private void OnEnable()
    {
        CurrentState?.OnEnable(this);

        Inputs.OnMoveEvent += Inputs_OnMoveEvent;
    }
    private void OnDisable()
    {
        CurrentState?.OnDisable(this);

        Inputs.OnMoveEvent -= Inputs_OnMoveEvent;
    }

    private void Update()
    {
        CurrentState?.UpdateState(this);

        Shader.SetGlobalVector("_PlayerPosition",transform.position);

        SpeedShow.text = $"Speed: {Mathf.Round(Vector3.Distance(Vector3.zero, Rigidbody.velocity)*20)/20}\n" +
            $"X : {Mathf.Round(Rigidbody.velocity.z*20)/20}\n" +
            $"Y : {Mathf.Round(Rigidbody.velocity.y*20)/20}";

        //Animator
        AnimatorPlayer.SetFloat("VelocityX", Mathf.Abs(Rigidbody.velocity.z/MovementSettings.maxMoveSpeed));
        AnimatorPlayer.SetFloat("VelocityY", Mathf.Clamp(Rigidbody.velocity.y/5, -1, 1));

    }
    private void Inputs_OnMoveEvent(Vector2 Dir)
    {
        DirectionInput = Dir;
    }
    public void SwitchState(S_basePlayerStates newState)
    {
        CurrentState?.OnDisable(this);
        CurrentState = newState;

        if (_debugState)
            Debug.Log($"New state: {CurrentState}");

        newState?.OnEnable(this);
        newState?.EnterState(this);
    }

    public bool CheckGrounded()
    {
        bool Ground = false;

        if (Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, MovementSettings.jumpResetLayers))
            Ground = true;
        else if(Physics.SphereCast(transform.position, 0.25f, Vector3.down, out _, _groundCheckDistance + 0.2f, MovementSettings.jumpResetLayers))
            Ground = true;

        AnimatorPlayer.SetBool("IsInTheAir", !Ground);

        return Ground;

        
    }

    public bool CheckWall(float distanceCheck)
    {
        Vector3 dir = FacingRight ? Vector3.forward : Vector3.back;
        Ray ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hit, distanceCheck, MovementSettings.wallJumpLayers))
        {
            Debug.DrawRay(ray.origin, ray.direction * distanceCheck, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distanceCheck, Color.red);
            return false;
        }
    }

    public void HandleFlip(float moveInput)
    {
        

        if (moveInput > 0 && !FacingRight)
        {
            SetFlip(0);
        }
        else if (moveInput < 0 && FacingRight)
        {
            SetFlip(180);
        }

        
    }

    public void SetFlip(int value)
    {
        S_hairFollow HairFollow = S_controllerPlayer.Instance.HairFollow;

        FacingRight = !FacingRight;

        HairFollow.FlipHair(FacingRight);

        Vector3 rot = _visualObject.transform.eulerAngles;
        rot.y = value;
        _visualObject.transform.eulerAngles = rot;
    }
    // 0.15 quand on chute
    //0.85 au sol
    public void AddAirDash(int Count)
    {
        AirDashCount += Count;
        AirDashCount = Mathf.Clamp(AirDashCount, 0, MovementSettings.MaxAirDashCount);
        SwitchColorAirDash();
    }

    public void SetAirDash(int Value)
    {
        AirDashCount = Value;
        AirDashCount = Mathf.Clamp(AirDashCount, 0, MovementSettings.MaxAirDashCount);
        SwitchColorAirDash();
    }

    private void SwitchColorAirDash()
    {
        if(!_airColorEnable && AirDashCount > 0){
            _airColorEnable = true;
            S_controllerPlayer.Instance.ColorSwitchHair.ColorSwitch(true);
            S_controllerPlayer.Instance.HairFollow.SetupPos();
            return;
        }

        if(_airColorEnable && AirDashCount <= 0)
        {
            _airColorEnable = false;
            S_controllerPlayer.Instance.ColorSwitchHair.ColorSwitch(false);
            S_controllerPlayer.Instance.HairFollow.SetUpSmallPos();
            return;
        }
    }

    public void SwitchVisual(bool Enable)
    {
        GameObject ObjectCorp = _visualObject.GetComponentInChildren<Transform>().gameObject;

        if (_disableSphereEffect is not null)
            StopCoroutine(_disableSphereEffect);


        if (Enable)
        { 
            ObjectCorp.SetActive(false);
            _hairEffect.enabled = false;
            _visualEffectSphereObject.gameObject.SetActive(true); 
            IsDashing = true;
            _visualEffectSphereObject.Play();
            _visualEffectSphereObject.SetVector2("DirectionSphere",new Vector2(DashDirection.y,DashDirection.z));

        }
        else
        {
            _visualEffectSphereObject.Stop();
            _hairEffect.enabled = true;
            S_controllerPlayer.Instance.HairFollow.ResetHair();
            ObjectCorp.SetActive(true);
            _disableSphereEffect = DisableSpherEffect();
            StartCoroutine(_disableSphereEffect);
        }

    }

    #region === Coroutine ===

    public void StartDureationPushBounds()
    {
        PressRebounds = true;

        if (_durationPressReboundsCoroutine is not null)
            StopCoroutine(_durationPressReboundsCoroutine);

        _durationPressReboundsCoroutine = DurationReboundsCoroutine(MovementSettings.reboundTimer);
        StartCoroutine(_durationPressReboundsCoroutine);

    }
    public void SetfalsePressRebounds()
    {
        if (_durationPressReboundsCoroutine is not null)
            StopCoroutine(_durationPressReboundsCoroutine);
        PressRebounds = false;
    }
    IEnumerator DurationReboundsCoroutine(float ReloadDuration)
    {
        yield return new WaitForSeconds(ReloadDuration);
        PressRebounds = false;
    }
    public void StartReloadGroundDash()
    {
        EnableGroundDash = false;

        if(_reloadGroundDashCoroutine is not null)
            StopCoroutine( _reloadGroundDashCoroutine );

        _reloadGroundDashCoroutine = ReloadGroundDash(MovementSettings.groundDashCooldownTime);
        StartCoroutine( _reloadGroundDashCoroutine );

    }
    public void SetTrueGroundDash()
    {
        if (_reloadGroundDashCoroutine is not null)
            StopCoroutine(_reloadGroundDashCoroutine);
        EnableGroundDash = true;
    }
    IEnumerator ReloadGroundDash(float ReloadDuration)
    {
        yield return new WaitForSeconds(ReloadDuration);
        EnableGroundDash = true;
    }

    public void StartReloadWallSlide()
    {
        EnableWallSlide = false;

        if (_reloadWallSlideCoroutine is not null)
            StopCoroutine(_reloadWallSlideCoroutine);

        _reloadWallSlideCoroutine = ReloadWallSlide(MovementSettings.SlideWallCooldownTimeAfterWallJump);
        StartCoroutine(_reloadWallSlideCoroutine);

    }
    public void SetTrueWallSlide()
    {
        if (_reloadWallSlideCoroutine is not null)
            StopCoroutine(_reloadWallSlideCoroutine);
        EnableWallSlide = true;
    }
    IEnumerator ReloadWallSlide(float ReloadDuration)
    {
        yield return new WaitForSeconds(ReloadDuration);
        EnableWallSlide = true;
    }

    IEnumerator DisableSpherEffect()
    {
        yield return new WaitForSeconds(1);
        _visualEffectSphereObject.gameObject.SetActive(false);
        IsDashing = false;
    }

    #endregion

    #region === Gizmos ===
    private void OnDrawGizmos()
    {
        if (Rigidbody is null) return;

        Gizmos.color = _groundRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);

        Gizmos.color = _wallRayColor;
        Vector3 wallDirection = FacingRight ? Vector3.forward : Vector3.back;
        Gizmos.DrawLine(transform.position, transform.position + wallDirection * WallCheckDistance);

        Gizmos.color = _ceilingRayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _groundCheckDistance);

        Gizmos.color = _bounceRayColor;
        Vector3 dashDirection = new Vector3(0, DirectionInput.y, DirectionInput.x).normalized;
        Gizmos.DrawLine(transform.position, transform.position + dashDirection * 5f);

        Gizmos.color = _velocityRayColor;
        Vector3 velocityDir = Rigidbody.velocity.normalized;
        Gizmos.DrawLine(transform.position, transform.position + velocityDir * 5f);

        //if (IsDashing)
        //{
        //    Gizmos.color = Color.magenta;
        //    Vector3 raycastDir = Rigidbody.velocity.normalized;
        //    Vector3 rayOrigin = transform.position + raycastDir * 0.25f;
        //    Gizmos.DrawLine(rayOrigin, rayOrigin + raycastDir * 1.5f);
        //}
    }

    #endregion
}
