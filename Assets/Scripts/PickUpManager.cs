using System;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    private Draggable pickedUpObject; 

    private void Start()
    {
        GameInput.Instance.OnCursorMove += GameInputOnOnCursorMove;
        GameInput.Instance.OnPickupActionStarted += GameInputOnOnPickupActionStarted;
		GameInput.Instance.OnPickupActionEnded += GameInputOnPickupActionEnded;
    }

	private void GameInputOnPickupActionEnded(object sender, EventArgs e)
	{
        if (pickedUpObject != null)
        {
            pickedUpObject.Drop();
            pickedUpObject = null;
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
                pickedUpObject.PickUp();
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
            var planeHeight = 0f;
            Plane ground = new Plane(Vector3.up, new Vector3(0, planeHeight, 0));


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
					var hoverHeight = hitPoint.y + hoverMargin;
                    pickedUpObject.Move(new Vector3(pickedUpObject.transform.position.x, hoverHeight, pickedUpObject.transform.position.z));
				}
			}
		}
    }
    
}
