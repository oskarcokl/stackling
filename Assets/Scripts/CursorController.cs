using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [Header("Sprites")] 
    [SerializeField] Sprite holdingCursorSprite;
    [SerializeField] Texture2D idleCursorSprite;
    [SerializeField] Texture2D releaseCursorSprite;
    
    [Header("UI Elements")]
    [SerializeField] private Image fakeCursorImage;
    
    [Header("Timers")]
    [SerializeField] private float releaseSpriteTime = 1;

    private Vector3 grabPos;
    
    public enum CursorState
    {
        Holding,
        Release,
        Idle,
    }
    
    private bool _isTracking = false;

    private void Start()
    {
        fakeCursorImage.enabled = false;
    }

    private void Update()
    {
        if (_isTracking)
        {
            var screenPos = Camera.main.WorldToScreenPoint(grabPos);
            fakeCursorImage.transform.position = screenPos;
        }
    }


    public void TrackBlock()
    {
        SetState(CursorState.Holding);
        _isTracking = true;
        // move the 
    }

    public void SetPosition(Vector3 position)
    {
        grabPos = position;
    }

    public void ReleaseBlock()
    {
        _isTracking = false;
        fakeCursorImage.enabled = false;
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
                // Switch to image rendering
                Cursor.visible = false;
                fakeCursorImage.enabled = true;
                break;
            case CursorState.Idle:
                Cursor.SetCursor(idleCursorSprite, new Vector2(5, 4), CursorMode.Auto);
                Cursor.visible = true;
                break;
            case CursorState.Release:
                Cursor.SetCursor(releaseCursorSprite, Vector2.zero, CursorMode.Auto);
                Cursor.visible = true;
                // go to release sprite
                break;
        }
    }
}