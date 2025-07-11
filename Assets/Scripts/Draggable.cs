using Unity.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
	// This script enables an object to be picked up and dragger around. 
	[SerializeField] private Transform raycastDownPoint;


	enum State
	{
		Idle,
		PickUp,
		Dragging,
	}

	private State _state;
	private Collider _collider;

	private void Awake()
	{
		_collider = GetComponent<Collider>();
		_state = State.Idle;
	}


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
			// Currently using the _collider.bounds.extends.y. This assumes that we are never going to rotate the object
			// also there might be other possbile sollutions to this I just didnt think of any.
			transform.position = new Vector3(transform.position.x, hitPoint.y + _collider.bounds.extents.y, transform.position.z);
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
		var originalLayer = gameObject.layer;
		
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		hitPoint = Vector3.zero;
		
		// NOTES: Not entirely sure if this is needed but it does seem to work so I'm gonna leave it in.
		RaycastHit highestCollider = new RaycastHit
		{
			distance = float.MaxValue
		};
		
		 
		var maxCastDistance = 10f;
		var extentPadding = .2f;

		var collisions = Physics.BoxCastAll(raycastDownPoint.position, _collider.bounds.extents + (Vector3.one * extentPadding), Vector3.down, transform.rotation, maxCastDistance);

		if (collisions.Length > 0)
		{
			highestCollider = collisions[0];
		}
		
		foreach (var collision in collisions )
		{
			if (collision.collider != _collider) // Ignore collision against self
			{
				// Hit collider that isnt the object itself.
				if (highestCollider.distance > collision.distance)
				{
					highestCollider = collision;
				}
			}
			// Don't early return in the else statement like a fucking idiot!
		}

		gameObject.layer = originalLayer;
		
		if (highestCollider.distance < float.MaxValue)
		{
			Debug.Log(highestCollider.transform.name);	
			hitPoint = highestCollider.point;
			return true;
		}
		

		return false;
	}

	public float GetHalfHeight()
	{
		return _collider.bounds.extents.y;	
	}
}
