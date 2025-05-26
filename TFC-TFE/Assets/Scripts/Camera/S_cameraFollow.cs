using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_cameraFollow : MonoBehaviour{
    public static Action<bool> OnSlowMotionStateChanged;

    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;

    [SerializeField] private float _distanceSpeed = 1;

    [Header("Positiontype")]
    [SerializeField] private Vector3 _positionGround;
    [SerializeField] private Vector3 _positionAir;
    [SerializeField] private float _speedTypeTransition = 5f;
    [SerializeField] private AnimationCurve _switchTypeCurve;

    [Header("RayGround")]
    [SerializeField] private float _maxDistanceRay = 50f;
    [SerializeField] private LayerMask _cameraColidder;
    [SerializeField] private float _distanceCameraGround = 2;
    [SerializeField] private AnimationCurve _groundDistanceModif;
    [SerializeField,Range(0,1)] private float _switchPourcentage;
    [SerializeField] bool _enableDistanceGround = true;

    [Header("Joystick")]
    [SerializeField] private float _additionalInputValue;
    [SerializeField] private AnimationCurve _moveInputStrength;

    [Header("Camera Zoom")]
    [SerializeField] private Camera _camera; 
    [SerializeField] private float _normalOrthoSize = 5f; 
    [SerializeField] private float _slowMotionOrthoSize = 8f; 
    [SerializeField] private float _zoomSpeed = 2f;

    private float _targetOrthoSize;
    private Vector3 _positionAdditive;
    private Vector3 _inputCamera;
    Inputs _input;

    private void OnEnable()
    {
        _input = new Inputs();
        _input.Camera.CameraMove.Enable();
        _input.Camera.CameraMove.performed += CameraMove_performed;
        _input.Camera.CameraMove.canceled += CameraMove_canceled;
        OnSlowMotionStateChanged += HandleSlowMotionState; 
    }
    private void OnDisable()
    {
        _input.Camera.CameraMove.performed -= CameraMove_performed;
        _input.Camera.CameraMove.canceled -= CameraMove_canceled;
        _input.Camera.CameraMove.Disable();
        OnSlowMotionStateChanged -= HandleSlowMotionState;
    }
    private void CameraMove_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _inputCamera = new Vector3(0, context.ReadValue<Vector2>().y, context.ReadValue<Vector2>().x);
    }

    private void CameraMove_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _inputCamera = Vector3.zero;
    }

    private void Start(){
        _targetOrthoSize = _normalOrthoSize;
        _camera.orthographicSize = _normalOrthoSize;
    }

    private void FixedUpdate()
    {
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetOrthoSize, _zoomSpeed * Time.deltaTime);

        float distance = Vector3.Distance(_player.transform.position, transform.position);
        distance *= _distanceSpeed;

        RaycastHit hit;

        

        float DistanceGround = 0;
        Vector3 PositionSelect = _positionAir;
        Vector3 InputCameraReader = _inputCamera;

        if (_enableDistanceGround && Physics.Raycast(_player.transform.position, Vector3.down, out hit, _maxDistanceRay, _cameraColidder))
        {
            DistanceGround = Vector3.Distance(_player.transform.position, hit.point)/ _maxDistanceRay;
            PositionSelect = Vector3.Lerp(_positionGround, _positionAir, _switchTypeCurve.Evaluate(DistanceGround));

            if (DistanceGround <= _switchPourcentage)
            {
                InputCameraReader = new Vector3(InputCameraReader.x, Mathf.Max(InputCameraReader.y, 0), InputCameraReader.z);
            }
        }
        
        InputCameraReader = InputCameraReader.normalized * _moveInputStrength.Evaluate(Vector3.Distance(InputCameraReader, Vector3.zero));

        PositionSelect -= Vector3.up * _groundDistanceModif.Evaluate(DistanceGround) * _distanceCameraGround;
        PositionSelect += InputCameraReader * _additionalInputValue;
        _positionAdditive = Vector3.MoveTowards(_positionAdditive, PositionSelect, _speedTypeTransition * Time.deltaTime);

        Vector3 targetPosition = _player.transform.position + _positionAdditive;

        Vector3 cameraPosition = Vector3.MoveTowards(transform.position, targetPosition, distance * _speed * Time.deltaTime);

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, cameraPosition.y, cameraPosition.z);
    }
    private void HandleSlowMotionState(bool isSlowMotion){
        _targetOrthoSize = isSlowMotion ? _slowMotionOrthoSize : _normalOrthoSize;
    }

}
