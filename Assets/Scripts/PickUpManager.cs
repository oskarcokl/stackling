using System;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    private Draggable pickedUpObject; 
    
    private void Start()
    {
        GameInput.Instance.OnCursorMove += GameInputOnOnCursorMove;
        GameInput.Instance.OnPickupAction += GameInputOnOnPickupAction;
    }

    private void GameInputOnOnPickupAction(object sender, EventArgs e)
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
        if (pickedUpObject)
        {
            Debug.Log("World pos: " +Camera.main.ScreenToWorldPoint(GameInput.Instance.GetCursorPosition()) );
            var cursorPos = GameInput.Instance.GetCursorPosition();
            pickedUpObject.Move(Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, 10)));
        }
    }
    
}
