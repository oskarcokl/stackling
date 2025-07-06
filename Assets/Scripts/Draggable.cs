using Unity.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // This script enables an object to be picked up and dragger around. 
    [SerializeField] private float hoverHeight; 
    [SerializeField] private Collider collider;
    
    
    enum State
    {
        Idle,
        PickUp,
        Dragging,
    } 
    
    private State _state;
    

    public void PickUp()
    {
        // Pick Up the object
        _state = State.PickUp;
        // Maybe we dont nee to se the hover here. Maybe we just need 
        // to set some sort of state to animate it.
        transform.position += Vector3.up * hoverHeight;
        _state = State.Dragging;
    }

    public void Move(Vector3 destination)
    {
        Debug.Log("Move to " + destination); 
        
        // Move the object to some location in the world
        // Keep in mind the hover
        var maxCastDistance = 20f;
        var extentPadding = 1f;
        transform.position = destination;
        
        if (Physics.BoxCast(transform.position, collider.bounds.extents + (Vector3.one * extentPadding), Vector3.down, out var hit, transform.rotation,
                maxCastDistance))
        {
            // If hit a collider then hover "hoverHeight" above it.
            transform.position += hit.point + Vector3.up * hoverHeight;;    
        }
        else
        {
            // Didn't hit collider hover hoverHeight above ground.
            transform.position += Vector3.up * hoverHeight;; 
        }
        _state = State.Dragging;
    }

    public void Drop()
    {
        // Drop the object on the ground. Maybe we can make this with or without gravity.
        // But for now. I think we should just do it without gravity since 
        // that is the original vision.
        _state = State.Idle;
    }

    public void Rotate(Quaternion rotateBy, Vector3 axis)
    {
        // Rotates the object around the specified axis by specified amount.
        // TODO: Implement
    }
}
