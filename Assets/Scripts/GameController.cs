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
    }

    public void Resume()
    {
        m_GameState = GameState.Running;
        GameInput.Instance.EnablePlayerInput();
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
