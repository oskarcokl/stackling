using UnityEngine;
using UnityEngine.InputSystem;

public class BlockDragger : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 4f;
    [SerializeField] private float raycastHeight = 5f;
    [SerializeField] private float liftSpeed = 10f;

    private Camera _cam;
    private GameObject _selectedBlock;
    private Vector3 _localGrabOffset;
    private float _targetY;
    private bool _isLifting;
    private InputSystem_Actions _input;

    private void OnEnable()
    {
        _cam = Camera.main;
        _input = new InputSystem_Actions();

        _input.Player.Click.started += ctx => TryPickBlock();
        _input.Player.Click.canceled += ctx => DropBlock();

        _input.Enable();
    }

    private void OnDisable() => _input.Disable();

    private void Update()
    {
        if (_selectedBlock == null) return;

        Vector2 mouseScreenPos = _input.Player.PointerPosition.ReadValue<Vector2>();
        Ray screenRay = _cam.ScreenPointToRay(mouseScreenPos);

        Vector3 currentGrabPoint = _selectedBlock.transform.TransformPoint(_localGrabOffset);
        Plane dragPlane = new Plane(Vector3.up, new Vector3(0, currentGrabPoint.y, 0));

        if (!dragPlane.Raycast(screenRay, out float enter)) return;

        Vector3 desiredMouseWorld = screenRay.GetPoint(enter);

        // Raycast from the block's current position downward to determine hover height
        Vector3 rayOrigin = new Vector3(_selectedBlock.transform.position.x, raycastHeight, _selectedBlock.transform.position.z);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
        {
            float groundY = hit.point.y;
            _targetY = groundY + hoverHeight;

            Vector3 worldTargetGrabPoint = new Vector3(desiredMouseWorld.x, _targetY, desiredMouseWorld.z);
            Vector3 currentGrabWorld = _selectedBlock.transform.TransformPoint(_localGrabOffset);
            Vector3 delta = worldTargetGrabPoint - currentGrabWorld;

            if (_isLifting)
            {
                float step = liftSpeed * Time.deltaTime;
                if (Mathf.Abs(currentGrabWorld.y - _targetY) < 0.01f)
                    _isLifting = false;
                else
                    delta.y = Mathf.Sign(delta.y) * Mathf.Min(Mathf.Abs(delta.y), step);
            }

            _selectedBlock.transform.position += delta;
        }
    }

    private void TryPickBlock()
    {
        var ray = _cam.ScreenPointToRay(_input.Player.PointerPosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Block"))
        {
            _selectedBlock = hit.collider.gameObject;
            _localGrabOffset = _selectedBlock.transform.InverseTransformPoint(hit.point);
            _selectedBlock.GetComponent<Rigidbody>().isKinematic = true;
            _isLifting = true;

            Cursor.visible = false;
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
            Cursor.visible = true;
        }
    }
}
