using UnityEngine;
using UnityEngine.InputSystem;

public class BlockDragger : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 4f; // how high above the surface to hover.
    [SerializeField] private float raycastHeight = 5f; // how high above the block to start raycasting.
    
    
    private Camera _cam;
    private GameObject _selectedBlock;
    private Vector3 _offset;
    private float _zCoord;

    private InputSystem_Actions _input;

    private void OnEnable()
    {
        _cam = Camera.main;
        _input = new InputSystem_Actions();

        _input.Player.Click.started += ctx => TryPickBlock();
        _input.Player.Click.canceled += ctx => DropBlock();
        
        _input.Enable();  
        
        
        print("Hover height: " + hoverHeight);
    } 
    private void OnDisable() => _input.Disable();

    // Update is called once per frame
    private void Update()
    {
        if (_selectedBlock != null)
        {
            var screenRay = _cam.ScreenPointToRay(_input.Player.PointerPosition.ReadValue<Vector2>());
            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(screenRay, out var enter))
            {
                var targetXZ = screenRay.GetPoint(enter);
                var rayOrigin = new Vector3(targetXZ.x, raycastHeight, targetXZ.z);

                var ray = new Ray(rayOrigin, Vector3.down);
                var hits = Physics.RaycastAll(ray);

                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject == _selectedBlock) continue;
                    var hoverPos = new Vector3(targetXZ.x, hit.point.y + hoverHeight, targetXZ.z);
                    print("Hover pos: " + hoverPos);
                    _selectedBlock.transform.position = hoverPos + _offset;
                    break;
                } 
            }
        } 
    }

    private void TryPickBlock()
    {
        var ray = _cam.ScreenPointToRay(_input.Player.PointerPosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Block"))
        {
            print(hit.collider.name);
            _selectedBlock = hit.collider.gameObject;
            
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out var enter))
            {
                var hitPoint = ray.GetPoint(enter);
                _offset = _selectedBlock.transform.position - hitPoint;
                print("Calculated offset: " + _offset);
            }
            
            _selectedBlock.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            print("No Block Selected");
        }
    }

    private void DropBlock()
    {
        if (_selectedBlock != null)
        {
            _selectedBlock.GetComponent<Rigidbody>().isKinematic = false;
            _selectedBlock = null;
        }
    }
}
