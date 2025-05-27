using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockDragger : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 4f;
    [SerializeField] private float liftSpeed = 10f;
    [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CursorController cursorController;
    [SerializeField] private PlacementIndicatorController placementIndicatorController;

    private Camera _cam;
    private GameObject _selectedBlock;
    private Vector3 _localGrabOffset;
    private Vector3 _initialPosition;
    private Vector3 _currentPosition;
    private float _targetY;
    private bool _isLifting;
    private bool _isDragging;
    private InputSystem_Actions _input;
    private int _originalLayer;
    private Tween _yTween;

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

        if (_isDragging)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            Vector3 right = _cam.transform.right;
            Vector3 forward = Vector3.ProjectOnPlane(_cam.transform.forward, Vector3.up).normalized;

            _currentPosition += (right * mouseDelta.x + forward * mouseDelta.y) * moveSpeed;

            Vector3 rayOrigin = new Vector3(_currentPosition.x, 100f, _currentPosition.z);
            
            
            var ray = new Ray(rayOrigin, Vector3.down);
            var hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject == _selectedBlock) continue;
                
                _targetY = hit.point.y + hoverHeight;
            }
            
            var currentPosition = _selectedBlock.transform.position; 
            Vector3 desiredPosition = new Vector3(_currentPosition.x, currentPosition.y, _currentPosition.z); // as not to interfeer with the tween.
            Vector3 currentGrabWorld = _selectedBlock.transform.TransformPoint(_localGrabOffset);
            Vector3 delta = desiredPosition - currentGrabWorld;
            delta.y = 0; // let the delta y be handled by DOTween
            
            if (Mathf.Abs(currentPosition.y - _targetY) > 0.01f)
            {
                _yTween?.Kill();
                _yTween = _selectedBlock.transform.DOMoveY(_targetY, 0.2f).SetEase(Ease.OutQuad);
            }
            
            cursorController.SetPosition(currentGrabWorld);
            
            _selectedBlock.transform.position += delta;
            
            placementIndicatorController.SetPoisition(_selectedBlock.transform.position);
        }
    }

    private void TryPickBlock()
    {
        var ray = _cam.ScreenPointToRay(_input.Player.PointerPosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Block"))
        {
            _selectedBlock = hit.collider.gameObject;

            _localGrabOffset = _selectedBlock.transform.InverseTransformPoint(hit.point);
            _initialPosition = hit.point;
            _currentPosition = _initialPosition;
            _selectedBlock.GetComponent<Rigidbody>().isKinematic = true;

            cursorController.TrackBlock();
            cursorController.SetPosition(hit.point);
            
            placementIndicatorController.ShowIndicator();

            _isLifting = true;
            _isDragging = true;
            _targetY = hoverHeight;
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
            _isDragging = false;
            if (Physics.Raycast(_selectedBlock.transform.position, Vector3.down, out RaycastHit hit))
            {
                var col = _selectedBlock.GetComponent<Collider>();
                var halfHeight = col.bounds.extents.y;
                print("Target y: "+ hit.point.y);
                _yTween?.Kill();
                _yTween = _selectedBlock.transform.DOMoveY(hit.point.y + halfHeight + 0.01f, 0.4f).SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    _selectedBlock.GetComponent<Rigidbody>().isKinematic = false;
                    _selectedBlock = null;
                });
            }
            else
            {
                // If for some reason we dont have anything beneth us just use let the physics system handle it.
                print("Else");
                _selectedBlock.GetComponent<Rigidbody>().isKinematic = false;
                _selectedBlock = null;

            }
            cursorController.ReleaseBlock();
            placementIndicatorController.HideIndicator();
        }
    }
}
