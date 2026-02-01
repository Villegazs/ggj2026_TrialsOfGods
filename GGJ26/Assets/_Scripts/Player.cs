using System;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Unity.Collections;

public class Player : MonoBehaviour , IDamageable
{
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnApplyDamage;
    public static Player Instance { get; private set; }
    
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private Animator animator;
    
    private CharacterMovement characterMovement;
    private PlayerStateMachine playerStateMachine;
    private int health;
    
    private bool isGamePaused = false;
    private bool isCursorLocked = false;
    
    [ReadOnly] private float timer;

    private void Awake()
    {
        isGamePaused = false;
        Instance = this;
        characterMovement = GetComponent<CharacterMovement>();
        health = (int)maxHealth;
        animator.enabled = true;
        isCursorLocked = true;
        CursorState(isCursorLocked);

    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        playerStateMachine = characterMovement.StateMachine;
    }
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        if(IsDead()) return;
        
        TogglePauseGame();
    }
    

    
    public void TakeDamage(int damage)
    {
        health -= damage;
        OnApplyDamage?.Invoke(this, EventArgs.Empty);
        Debug.Log($"Player took {damage} damage, remaining health: {health}");
        if (IsDead())
        {
            CursorState(false);
            StaticEventHandler.RaiseDeath();
            animator.enabled = false;
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }
    public float GetHealthNormalized()
    {
        return health/(float) maxHealth;
    }
    
    
    private void CursorState(bool isLocked)
    {
        Debug.Log($"Cursor lock: {isCursorLocked}");
        isCursorLocked = isLocked;
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

    public float OnTimerRunningMaskCooldown()
    {
        timer = characterMovement.TimerValue;
        return timer;
    }
    public CharacterMovement GetCharacterMovement()
    {
        return characterMovement;
    }
    
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused; ;
        CursorState(!isGamePaused);
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
}
