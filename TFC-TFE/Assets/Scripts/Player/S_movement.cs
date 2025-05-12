using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class S_movement : MonoBehaviour{
    [Header("Paramètres de déplacement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _dashForce = 10f;
    [SerializeField] private float _dashDuration = 0.2f;
    [SerializeField] private bool _canJump = true;

    [Header("Paramètres physiques")]
    [SerializeField] private Rigidbody _rb;

    [Header("Paramètres des commandes")]
    [SerializeField] private S_Commandes _inputSettings;

    [Header("Paramètres de rotation")]
    [SerializeField] private GameObject _objectToFlip;

    [Header("Paramètres de slowmotion")]
    [SerializeField] private float _intensiteSlow = 0.5f;
    [SerializeField] private float _timerSlow = 10f;

    private bool _isGrounded = true;
    private bool _isDashing = false;
    private bool _facingRight = true;
    private bool _isSlowMotionActive = false;
    private bool _isAirDashReady = false;

    private Inputs _inputs;

    private void Start(){
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
        //_inputs.Player.Direction.performed += ;
    }

    private void Update(){
        if (_isDashing) return;

        float moveInput = 0f;
        if (Input.GetKey(_inputSettings.moveForward)){
            moveInput = 1f;
        }else if (Input.GetKey(_inputSettings.moveBackward)){
            moveInput = -1f;
        }

        Vector3 move = new Vector3(0, 0, moveInput * _moveSpeed);
        _rb.velocity = new Vector3(0, _rb.velocity.y, move.z);

        if (moveInput > 0 && !_facingRight){
            Flip();
        }else if (moveInput < 0 && _facingRight){
            Flip();
        }

        if (_canJump && _isGrounded && Input.GetKeyDown(_inputSettings.jump)){
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }

        if (!_isGrounded){
            HandleAirDash(moveInput);
        }else if (Input.GetKeyDown(_inputSettings.dash))
        {
            Dash(moveInput);
        }
    }
    private void HandleAirDash(float direction){
        if (Input.GetKey(_inputSettings.dash)){
            if (!_isSlowMotionActive){
                StartSlowMotion(0.2f);
            }
            _isSlowMotionActive = true;
            _isAirDashReady = true;
        }

        if (Input.GetKeyUp(_inputSettings.dash) && _isAirDashReady)
        {
            StopSlowMotion();
            Dash(direction);
            _isAirDashReady = false;
        }
    }

    private void StartSlowMotion(float intensite){
        Time.timeScale = intensite;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void StopSlowMotion(){
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void Dash(float direction){
        _isDashing = true;

        Vector3 dashDirection;
        if (direction != 0){
            dashDirection = new Vector3(0, 0, direction).normalized;
        }else{
            if (_facingRight){
                dashDirection = Vector3.forward;
            }else{
                dashDirection = -Vector3.forward;
            }

        }

        _rb.AddForce(dashDirection * _dashForce, ForceMode.Impulse);

        StartCoroutine(EndDash());
    }

    private IEnumerator EndDash(){
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            _isGrounded = true;
        }
    }
    private void Flip(){
        _facingRight = !_facingRight;

        Vector3 rotation = _objectToFlip.transform.eulerAngles;
        rotation.y += 180f;
        _objectToFlip.transform.eulerAngles = rotation;
    }

    public void SlowMotion(float intensite, float duree){
        StartCoroutine(SlowMotionCoroutine(intensite, duree));
    }

    private IEnumerator SlowMotionCoroutine(float intensite, float duree){
        Time.timeScale = intensite;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duree);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

}