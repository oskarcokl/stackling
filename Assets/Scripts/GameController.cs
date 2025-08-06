using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private enum GameState
    {
        Running,
        Paused
    }

    private GameState m_GameState;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_GameState = GameState.Running;
    }


    public void Pause()
    {
        m_GameState = GameState.Paused;
        GameInput.Instance.DisablePlayerInput();
        GameInput.Instance.DisableCameraInput();
    }

    public void Resume()
    {
        m_GameState = GameState.Running;
        GameInput.Instance.EnablePlayerInput();
        GameInput.Instance.EnableCameraInput();
    }

    public bool IsPaused()
    {
        return m_GameState == GameState.Paused;
    }

    public bool IsRunning()
    {
        return m_GameState == GameState.Running;
    }
}
