using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransofrm;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float rotationSpeedX = 5f;
    [SerializeField] private float rotationSpeedY = 5f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float minVerticalAngle = -30f;

    private InputSystem_Actions _input;

    private Vector2 _lookInput = Vector2.zero;
    private float _zoomInput = 0f;

    private float _yAngle = 0f;
    private float _xAngle = 0f;
    private float _distance = 0f;

    private float _targetDistance = 0f;
    private float _targetYAngle = 0f;
    private float _targetXAngle = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var angles = transform.eulerAngles;
        _yAngle = angles.y;
        _xAngle = angles.x;

        _distance = minZoom;

        GameInput.Instance.OnCameraLookActionPerformed += GameInputOnCameraLookActionPerformed;
        GameInput.Instance.OnCameraLookActionCanceled += GameInputOnCameraLookActionCanceled;
        GameInput.Instance.OnCameraZoomActionPerformed += GameInputOnCameraZoomActionPerformed;
        GameInput.Instance.OnCameraZoomActionCanceled += GameInputOnCameraZoomActionCanceled;
    }

    private void GameInputOnCameraLookActionPerformed(object sender, GameInput.OnCameraLookActionPerformedEventArgs e)
    {
        _lookInput = e.lookVector;
    }

    private void GameInputOnCameraLookActionCanceled(object sender, EventArgs e)
    {
        _lookInput = Vector2.zero;
    }

    private void GameInputOnCameraZoomActionPerformed(object sender, GameInput.OnCameraZoomActionPerformedEventArgs e)
    {
        _zoomInput = e.zoomAmount;
    }

    private void GameInputOnCameraZoomActionCanceled(object sender, EventArgs e)
    {
        _zoomInput = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameInput.Instance.IsCameraLookButtonPressed() && !PickUpManager.Instance.IsObjectPickedUp())
        {
            _targetYAngle -= _lookInput.y * rotationSpeedY * Time.deltaTime;
            _targetXAngle += _lookInput.x * rotationSpeedX * Time.deltaTime;

            _targetYAngle = Math.Clamp(_targetYAngle, minVerticalAngle, maxVerticalAngle);

            _yAngle = Mathf.LerpAngle(_yAngle, _targetYAngle, 1f);
            _xAngle = Mathf.LerpAngle(_xAngle, _targetXAngle, .1f);

            transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0f);
        }

        if (Math.Abs(_zoomInput) >= 0.01f)
        {
            var dir = cameraTransofrm.localPosition.normalized;
            _targetDistance = cameraTransofrm.localPosition.magnitude;

            _targetDistance -= _zoomInput * zoomSpeed * Time.deltaTime;
            _targetDistance = Mathf.Clamp(_targetDistance, minZoom, maxZoom);
            _distance = Mathf.Lerp(_distance, _targetDistance, .5f);

            cameraTransofrm.localPosition = dir * _distance;
        }
    }
}
