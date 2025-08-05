using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    private bool m_IsVisible;
    private UIDocument m_UIDocument;

    private void Awake()
    {
        m_UIDocument = GetComponent<UIDocument>();
        Hide();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseUnpauseAction += GameInputOnPauseUnpauseAction;
    }

    private void GameInputOnPauseUnpauseAction(object sender, EventArgs e)
    {
        Debug.Log("PauseMenuController OnPauseUnpauseAction");
        if (m_IsVisible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Hide()
    {
        m_UIDocument.enabled = false;
        m_IsVisible = false;
    }

    private void Show()
    {
        m_UIDocument.enabled = true;
        m_IsVisible = true;
    }

}
