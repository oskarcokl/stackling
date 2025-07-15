using UnityEngine;

public class PlacementIndicatorVisual : MonoBehaviour
{
    [SerializeField] private Draggable draggable;
    
    private void Update()
    {
        if (Physics.BoxCast(draggable.transform.position, draggable.GetExtents(), Vector3.down, out RaycastHit hit))
        {
            transform.localPosition = Vector3.down * hit.distance;
        }
    }  
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false); 
    }
}
