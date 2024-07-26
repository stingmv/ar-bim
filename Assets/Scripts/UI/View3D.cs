using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using TouchPhase = UnityEngine.TouchPhase;

public class View3D : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    

    #region Rotation

    [Header("Rotation")]
    [SerializeField] private float _speedOrbitCamera;
    [SerializeField] private float _speedFreeCamera;
    private RaycastHit _hit;
    bool _pressed;
    private bool _isHit;
    private float _entradaY;
    private float _entradaYFreeCamera;
    private float _entradaX;
    private float _entradaXFreeCamera;
    private Vector3 _pivote;
    private Vector2 initialTouchPos;
    private bool _canRotate;
    private bool _useOrbit;
    private bool _hasUiInteractionBefore;
    public bool UseOrbit
    {
        get => _useOrbit;
        set => _useOrbit = value;
    }

    #endregion

    #region Movement
    
    [Header("Movement")]
    [SerializeField] private float _speedMovement;

    private Vector2 lastDiference;
    private float _maxDistance  = 1000f;
    private float _minSpeed = .05f;
    private float _maxSpeed = 50f;
    private float _distanceCameraToTarget;
    private float _difference;
    

    #endregion
    
    private void Update()
    {
        var touchOverUI = EventSystem.current.IsPointerOverGameObject();
        if (Input.touchCount != 0 && touchOverUI)
        {
            _hasUiInteractionBefore = true;

        }
        else if (Input.touchCount == 0)
        {
            _hasUiInteractionBefore = false;

        }

        if (Input.touchCount == 0 || touchOverUI || _hasUiInteractionBefore)
        {
            return;
        }

        var touchOne = Touchscreen.current.primaryTouch;

        if (Input.touchCount == 1)
        {
            if (touchOne.phase.ReadValue()  == UnityEngine.InputSystem.TouchPhase.Began )
            {
                if (touchOverUI)
                {
                    _canRotate = false;
                    return;
                }
                _canRotate = true;
                Ray ray = _camera.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out _hit))
                {
                    if (!_pressed)
                    {
                        _pivote = _hit.point;
                    }
                    _isHit = true;
                }
                else
                {
                    _isHit = false;
                }
            }
            else if (touchOne.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                if (!_canRotate)
                {
                    return;
                }
                if (!_isHit)
                {
                    _entradaYFreeCamera =   Input.GetTouch(0).deltaPosition.x * Time.deltaTime * _speedFreeCamera;
                    _entradaXFreeCamera = - Input.GetTouch(0).deltaPosition.y * Time.deltaTime * _speedFreeCamera;
                    _camera.transform.Rotate(_entradaXFreeCamera,_entradaYFreeCamera,0);
                    var eulAngle =_camera.transform.eulerAngles;
                    eulAngle.z = 0;
                    _camera.transform.eulerAngles = eulAngle;
                }
                else
                {
                    if (UseOrbit)
                    {
                        _entradaY =   Input.GetTouch(0).deltaPosition.x * _speedOrbitCamera;
                        _entradaX = - Input.GetTouch(0).deltaPosition.y * _speedOrbitCamera;
        
                        _camera.transform.RotateAround(_pivote, Vector3.up, _entradaY);
                        _camera.transform.RotateAround(_pivote, _camera.transform.right, _entradaX);
                    }
                    else
                    {
                        _entradaYFreeCamera =  Input.GetTouch(0).deltaPosition.x * Time.deltaTime * _speedFreeCamera;
                        _entradaXFreeCamera = - Input.GetTouch(0).deltaPosition.y * Time.deltaTime * _speedFreeCamera;
                        _camera.transform.Rotate(_entradaXFreeCamera,_entradaYFreeCamera,0);
                        var eulAngle =_camera.transform.eulerAngles;
                        eulAngle.z = 0;
                        _camera.transform.eulerAngles = eulAngle;
                    }
                    
                }
            }
            return;
        }
        if (Input.touchCount == 2)
        {
            
            var onePosition = Input.GetTouch(0);
            var twoPosition = Input.GetTouch(1);
            var diference = twoPosition.position - onePosition.position;
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                lastDiference = diference;
            }
            var percentage = 1 - lastDiference.sqrMagnitude / diference.sqrMagnitude;
            _distanceCameraToTarget = Vector3.Distance(_camera.transform.position, ModelGLTF.Instance.GetAssetUnloader().transform.position);
            _difference = _distanceCameraToTarget / _maxDistance;
            // float speed = Mathf.Lerp(0, maxSpeed, difference);
            float speed = _minSpeed + (_maxSpeed - _minSpeed) * _difference;;
            _speedMovement = speed;
            if (Mathf.Abs(percentage) < .04f)
            {
                Vector3 actualPosition =Input.GetTouch(1).deltaPosition;
                
                var predicedMovement = _camera.transform.TransformPoint(-actualPosition * _speedMovement);
                var smoothMovement = Vector3.Lerp(_camera.transform.position, predicedMovement,Time.deltaTime );
                _camera.transform.position = smoothMovement;
                // _camera.transform.position = _camera.transform.TransformPoint(- actualPosition * (Time.deltaTime * _speedMovement * difference));
            }
            else
            {
                _camera.transform.position = _camera.transform.TransformPoint(Vector3.forward * (percentage * 310 * Time.deltaTime * _speedMovement));
            }
            // Calcular actual posicion
            lastDiference = diference;
        }
    }
}