using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [Header("Sprites")] 
    [SerializeField] Texture2D holdingCursorSprite;
    [SerializeField] Texture2D idleCursorSprite;
    [SerializeField] Texture2D releaseCursorSprite;
    
    [Header("Timers")]
    [SerializeField] private float releaseSpriteTime = 1;

    private Vector3 grabPos;
    
    public enum CursorState
    {
        Holding,
        Release,
        Idle,
    }


    public void Start()
    {
        PickUpManager.Instance.OnPickUp += PickUpManagerOnOnPickUp;
        PickUpManager.Instance.OnDrop += PickUpManagerOnOnDrop;
        
    }

    private void PickUpManagerOnOnDrop(object sender, EventArgs e)
    {
        SetState(CursorState.Release);
        // Invoke(nameof(SetState), 0.1f);
        // wait 0.5 sec
        DelayedCall(releaseSpriteTime, () =>
        {
            SetState(CursorState.Idle);
        });
        
    }

    private void PickUpManagerOnOnPickUp(object sender, EventArgs e)
    {
        SetState(CursorState.Holding);
    }

    public void TrackBlock()
    {
        SetState(CursorState.Holding);
    }

    public void SetPosition(Vector3 position)
    {
        grabPos = position;
    }

    public void ReleaseBlock()
    {
        Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(grabPos));
        // move actual cursor to correct position
        SetState(CursorState.Release);
        // Invoke(nameof(SetState), 0.1f);
        // wait 0.5 sec
        DelayedCall(releaseSpriteTime, () =>
        {
            SetState(CursorState.Idle);
        });
        //SetState(CursorState.Idle);
    }
    
    private void DelayedCall(float delay, Action callback)
    {
        StartCoroutine(DelayedCoroutine(delay, callback));
    }

    private IEnumerator DelayedCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
 
    
    

    void SetState(CursorState state)
    {
        switch (state)
        {
            case CursorState.Holding:
                Cursor.SetCursor(holdingCursorSprite, Vector2.zero, CursorMode.Auto);
                break;
            case CursorState.Idle:
                Cursor.SetCursor(idleCursorSprite, new Vector2(5, 4), CursorMode.Auto);
                break;
            case CursorState.Release:
                Cursor.SetCursor(releaseCursorSprite, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}