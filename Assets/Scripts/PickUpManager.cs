using System;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
	public static PickUpManager Instance { get; private set; }
	public event EventHandler OnPickUp;
	public event EventHandler OnDrop;
	
    private Draggable pickedUpObject;

    private float planeHeight = 0f;

    public void Awake()
    {
		Instance = this; 
    }

    private void Start()
    {
        GameInput.Instance.OnCursorMove += GameInputOnOnCursorMove;
        GameInput.Instance.OnPickupActionStarted += GameInputOnOnPickupActionStarted;
		GameInput.Instance.OnPickupActionEnded += GameInputOnPickupActionEnded;
		GameInput.Instance.OnRotateLeftAction += GameInputOnOnRotateLeftAction;
		GameInput.Instance.OnRotateRightAction += GameInputOnOnRotateRightAction;
    }
    
    private void GameInputOnOnRotateRightAction(object sender, EventArgs e)
    {
	    if (pickedUpObject != null)
	    {
		    pickedUpObject.RotateRight();
	    }
    }

    private void GameInputOnOnRotateLeftAction(object sender, EventArgs e)
    {
	    if (pickedUpObject != null)
	    {
		    pickedUpObject.RotateLeft();
	    }
    }

    private void GameInputOnPickupActionEnded(object sender, EventArgs e)
	{
        if (pickedUpObject != null)
        {
            pickedUpObject.Drop();
            pickedUpObject = null;
            OnDrop?.Invoke(this, EventArgs.Empty);
        }
	}

	private void GameInputOnOnPickupActionStarted(object sender, EventArgs e)
    {
        var ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetCursorPosition());
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if (hit.transform.TryGetComponent(out Draggable draggable))
            {
                pickedUpObject = draggable;
                // This nicely solve the problem of objects moving in x and z when they are picked up
                // when their y is not close to the ground floor
                planeHeight = hit.point.y;
                pickedUpObject.PickUp();
                OnPickUp?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GameInputOnOnCursorMove(object sender, EventArgs e)
    {
        // We are going to accept drift. The base hover plane will be 2f above 0. 
        // We will still check under the object for collision but we will be ignoring the
        // floor since that will be covered by the hover plane.
        if (pickedUpObject != null)
        {
            Plane ground = new Plane(Vector3.up, new Vector3(0f, planeHeight, 0f));


            var cursorPos = GameInput.Instance.GetCursorPosition();
            var ray = Camera.main.ScreenPointToRay(cursorPos);
    
            if (ground.Raycast(ray, out float enter))
            {
                var cursorWorldPosition = ray.GetPoint(enter);

				// If collider underneath than mover
				pickedUpObject.Move(new Vector3(cursorWorldPosition.x, pickedUpObject.transform.position.y, cursorWorldPosition.z));

				if (pickedUpObject.ColliderUnderneath(out Vector3 hitPoint))
				{
					var hoverMargin = 1f;
					var hoverHeight = hitPoint.y + (pickedUpObject.GetHeight() * 1.5f) + hoverMargin;
                    pickedUpObject.Move(new Vector3(cursorWorldPosition.x, hoverHeight, cursorWorldPosition.z));
				}
			}
		}
    }
	
	public bool IsObjectPickedUp() 
	{
		return pickedUpObject != null;
	}
}
