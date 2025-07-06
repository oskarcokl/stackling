using UnityEngine;

public class InputContext : MonoBehaviour
{
    public static InputContext Instance { get; private set; } 
    
    public bool IsDraggingBlock { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    public void StartDraggingBlock() => IsDraggingBlock = true;
    public void StopDraggingBlock() => IsDraggingBlock = false;
}
