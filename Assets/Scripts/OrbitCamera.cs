using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransofrm; 
    [SerializeField] private float _maxZoom = 20f;
    [SerializeField] private float _minZoom = 2f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _maxVerticalAngle = 60f;
    [SerializeField] private float _minVerticalAngle = -30f;
    
    private InputSystem_Actions _input;
    
    private Vector2 _lookInput = Vector2.zero;
    private float _zoomInput = 0f;

    private float verticalAngle = 0f;
    private float horizontalAngle = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _input = new InputSystem_Actions();

        _input.Camera.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _input.Camera.Look.canceled += _ => _lookInput = Vector2.zero;
        
        
        _input.Camera.Zoom.performed += ctx => _zoomInput = ctx.ReadValue<Vector2>().y;
        _input.Camera.Zoom.canceled += _ => _zoomInput = 0f;
        
        _input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed && !InputContext.Instance.IsDragginBlock)
        {
            verticalAngle -= _lookInput.y * _rotationSpeed * Time.deltaTime;
            horizontalAngle += _lookInput.x * _rotationSpeed * Time.deltaTime;
            
            verticalAngle = Math.Clamp(verticalAngle, _minVerticalAngle, _maxVerticalAngle);
            
            transform.rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
        }

        if (Math.Abs(_zoomInput) >= 0.01f)
        {
            var dir = _cameraTransofrm.localPosition.normalized;
            var dist = _cameraTransofrm.localPosition.magnitude;
            
            dist -= _zoomInput * _zoomSpeed * Time.deltaTime;
            dist = Mathf.Clamp(dist, _minZoom, _maxZoom);
            _cameraTransofrm.localPosition = dir * dist;
        }

        // Is this needed?
        _zoomInput = 0f;
    }
}
