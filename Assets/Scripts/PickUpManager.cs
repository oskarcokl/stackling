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
        Debug.Log("Pick up action");
        var ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetCursorPosition());
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
        {
            if (hit.transform.TryGetComponent(out Draggable draggable))
            {
                Debug.Log("Pick up");
                pickedUpObject = draggable;
                pickedUpObject.PickUp();
            }
        }
    }

    private void GameInputOnOnCursorMove(object sender, EventArgs e)
    {
        if (pickedUpObject != null)
        {
            var cursorPos = GameInput.Instance.GetCursorPosition();
            var ray = Camera.main.ScreenPointToRay(cursorPos);
            Plane ground = new Plane(Vector3.up, new Vector3(0, 2f, 0));
    
            if (ground.Raycast(ray, out float enter))
            {
                var lastHit = ray.GetPoint(enter);

				pickedUpObject.Move(lastHit);
            }
        }
    }
    
}
