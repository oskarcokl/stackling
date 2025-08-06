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
    }

    private void PauseUnpauseOnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("OnPause action performed");
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

    public Vector3 GetCursorPosition()
    {
        return _cursorPosition;
    }
}
