using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnCursorMove;
    public class OnCursorMoveEventArgs : EventArgs
    {
        public Vector2 cursorPosition;
    }
    public event EventHandler<OnCameraLookActionPerformedEventArgs> OnCameraLookActionPerformed;
    public class OnCameraLookActionPerformedEventArgs : EventArgs
    {
        public Vector2 lookVector;
    }
    public event EventHandler OnCameraLookActionCanceled;
    public event EventHandler<OnCameraZoomActionPerformedEventArgs> OnCameraZoomActionPerformed;
    public class OnCameraZoomActionPerformedEventArgs : EventArgs
    {
        public float zoomAmount;
    }
    public event EventHandler OnCameraZoomActionCanceled;

    public event EventHandler OnPickupActionStarted;
    public event EventHandler OnPickupActionEnded;
    public event EventHandler OnRotateRightAction;
    public event EventHandler OnRotateLeftAction;
    public event EventHandler OnPauseUnpauseAction;


    // private InputSystem_Actions.PlayerInputActions _playerInputActions;
    private InputSystem_Actions _input;
    private Vector3 _cursorPosition;

    private void Awake()
    {
        Instance = this;

        _input = new InputSystem_Actions();

        EnablePlayerInput();
        _input.PlayerInput.PickUp.started += PickUpOnStarted;
        _input.PlayerInput.PickUp.canceled += PickUpOnCanceled;
        _input.PlayerInput.CursorMove.performed += CursorMoveOnPerformed;
        _input.PlayerInput.RotateLeft.performed += RotateLeftOnPerformed;
        _input.PlayerInput.RotateRight.performed += RotateRightOnPerformed;

        _input.UI.Enable();
        _input.UI.PauseUnpause.performed += PauseUnpauseOnPerformed;


        _input.Enable();
        _input.Camera.Look.performed += CameraLookOnPerformed;
        _input.Camera.Look.canceled += CameraLookOnCanceled;
        _input.Camera.Zoom.performed += CameraZoomOnPerformed;
        _input.Camera.Zoom.canceled += CameraZoomOnCanceled;
    }

    private void CameraLookOnPerformed(InputAction.CallbackContext obj)
    {
        OnCameraLookActionPerformed?.Invoke(this, new OnCameraLookActionPerformedEventArgs { lookVector = obj.ReadValue<Vector2>() });
    }
    private void CameraLookOnCanceled(InputAction.CallbackContext obj)
    {
        OnCameraLookActionCanceled?.Invoke(this, EventArgs.Empty);
    }
    private void CameraZoomOnPerformed(InputAction.CallbackContext obj)
    {
        OnCameraZoomActionPerformed?.Invoke(this, new OnCameraZoomActionPerformedEventArgs { zoomAmount = obj.ReadValue<Vector2>().y });
    }
    private void CameraZoomOnCanceled(InputAction.CallbackContext obj)
    {
        OnCameraZoomActionCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void PauseUnpauseOnPerformed(InputAction.CallbackContext obj)
    {
        OnPauseUnpauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void RotateRightOnPerformed(InputAction.CallbackContext obj)
    {
        OnRotateRightAction?.Invoke(this, EventArgs.Empty);
    }

    private void RotateLeftOnPerformed(InputAction.CallbackContext obj)
    {
        OnRotateLeftAction?.Invoke(this, EventArgs.Empty);
    }


    private void CursorMoveOnPerformed(InputAction.CallbackContext obj)
    {
        // REMINDER: Maybe we will have to send the mouse coords when the
        // action is invoked. Will see how the program develops.
        var pos = obj.ReadValue<Vector2>();
        _cursorPosition.x = pos.x;
        _cursorPosition.y = pos.y;
        OnCursorMove?.Invoke(this, EventArgs.Empty);
    }

    private void PickUpOnCanceled(InputAction.CallbackContext obj)
    {
        OnPickupActionEnded?.Invoke(this, EventArgs.Empty);
    }

    private void PickUpOnStarted(InputAction.CallbackContext obj)
    {
        //Debug.Log("Pick up action");
        OnPickupActionStarted?.Invoke(this, EventArgs.Empty);
    }

    public void DisablePlayerInput()
    {
        _input.PlayerInput.Disable();
    }

    public void EnablePlayerInput()
    {
        _input.PlayerInput.Enable();
    }

    public void DisableCameraInput()
    {
        _input.Camera.Disable();
    }

    public void EnableCameraInput()
    {
        _input.Camera.Enable();
    }

    public bool IsCameraLookButtonPressed()
    {
        return _input.Camera.LookButton.IsPressed();
   }

    public Vector3 GetCursorPosition()
    {
        return _cursorPosition;
    }
}
