using UnityEngine;
using System;

public class TrialsOfGodGameManager : MonoBehaviour
{
    public static TrialsOfGodGameManager Instance { get; private set; }
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    [SerializeField] private Loader.Scene nextLevelScene;
    [SerializeField] private CompleteLevel completeLevelCollider;
    
    private bool isPlayerDead = false;
    private bool isGamePaused = false;
    private bool isCursorLocked => !isGamePaused;
    

    private void Awake()
    {
        Instance = this;
        isGamePaused = false;
        CursorState(isGamePaused);
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        StaticEventHandler.OnDeath += StaticEventHandler_OnDeath;
        completeLevelCollider.OnLevelCompleted += CompleteLevelCollider_OnLevelCompleted;
    }
    private void StaticEventHandler_OnDeath()
    {
        isPlayerDead = true;
        isGamePaused = true;
        CursorState(isGamePaused);
    }
    private void CompleteLevelCollider_OnLevelCompleted(object sender, EventArgs e)
    {
        if(isPlayerDead) return;
        Loader.Load(nextLevelScene);
    }
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if (isPlayerDead) return;
        TogglePauseGame();
    }
    private void CursorState(bool isLocked)
    {
        Debug.Log($"Cursor lock: {isCursorLocked}");
        if(isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused; ;
        CursorState(isGamePaused);
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
        StaticEventHandler.OnDeath -= StaticEventHandler_OnDeath;
        completeLevelCollider.OnLevelCompleted -= CompleteLevelCollider_OnLevelCompleted;
    }
}
