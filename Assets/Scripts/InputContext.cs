using UnityEngine;

public class InputContext : MonoBehaviour
{
    public static InputContext Instance { get; private set; } 
    
    public bool IsDragginBlock { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
    public void StartDraggingBlock() => IsDragginBlock = true;
    public void StopDraggingBlock() => IsDragginBlock = false;
}
