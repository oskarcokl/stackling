using Unity.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
	// This script enables an object to be picked up and dragger around. 
	[SerializeField] private Collider collider;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private Transform raycastDownPoint;


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
		//transform.position += Vector3.up * hoverHeight;
		_state = State.Dragging;
	}

	public void Move(Vector3 destination)
	{
		// Move the object to some location in the world
		// Keep in mind the hover
		var maxCastDistance = 20f;
		var extentPadding = 1f;
		transform.position = destination;

		//if (Physics.BoxCast(transform.position, collider.bounds.extents + (Vector3.one * extentPadding), Vector3.down, out var hit, transform.rotation,
		//        maxCastDistance))
		//{
		//    // If hit a collider then hover "hoverHeight" above it.
		//    transform.position += hit.point + Vector3.up * hoverHeight; ;
		//}
		//else
		//{
		//    // Didn't hit collider hover hoverHeight above ground.
		//    transform.position += Vector3.up * hoverHeight; ;
		//}
		_state = State.Dragging;
	}

	public void Drop()
	{
		// Drop the object on the ground. Maybe we can make this with or without gravity.
		// But for now. I think we should just do it without gravity since 
		// that is the original vision.
	
		if (ColliderUnderneath(out var hitPoint))
		{
			// TODO: currently hardcoding the half width to correctly set the position
			// this needs to be somehow done programatically
			// We could have a a point at the bottom of the object and move that?
			transform.position = new Vector3(transform.position.x, hitPoint.y + 0.5f, transform.position.z);
		}
		else
		{
			var defaultY = 1f;
			transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);
		}

		_state = State.Idle;
	}

	public void Rotate(Quaternion rotateBy, Vector3 axis)
	{
		// Rotates the object around the specified axis by specified amount.
		// TODO: Implement
	}

	public bool ColliderUnderneath(out Vector3 hitPoint)
	{
		// Default value. Only change if actually colliding with something
		hitPoint = Vector3.zero;

		var maxCastDistance = 3f;
		var extentPadding = .2f;

		var collisions = Physics.BoxCastAll(raycastDownPoint.position, collider.bounds.extents + (Vector3.one * extentPadding), Vector3.down, transform.rotation, maxCastDistance);
		foreach (var collision in collisions )
		{
			if (collision.collider != collider) // Ignore collision against self
			{
				// Hit collider that isnt the object itself.
				hitPoint = collision.point;
				return true;
			}
			else
			{
				// Didn't hit any collider.
				return false;
			}
		}

		return false;
	}
}
