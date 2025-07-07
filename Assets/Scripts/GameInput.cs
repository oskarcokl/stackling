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


    // private InputSystem_Actions.PlayerInputActions _playerInputActions;
    private InputSystem_Actions _input;
    private Vector3 _cursorPosition;
    
    private void Awake()
    {
        Instance = this;

        _input = new InputSystem_Actions();
        
        _input.PlayerInput.Enable();
        
        _input.PlayerInput.PickUp.started += PickUpOnStarted;
		_input.PlayerInput.PickUp.canceled += PickUpOnCanceled;
        _input.PlayerInput.CursorMove.performed += CursorMoveOnPerformed;
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

    public Vector3 GetCursorPosition()
    {
        return _cursorPosition;
    }
}
