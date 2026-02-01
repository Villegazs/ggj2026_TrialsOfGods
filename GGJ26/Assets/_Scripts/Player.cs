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
    public event EventHandler OnApplyDamage;
    public static Player Instance { get; private set; }
    
    [SerializeField] private float maxHealth = 5;
    
    
    private CharacterMovement characterMovement;
    private PlayerStateMachine playerStateMachine;
    private int health;
    
    private bool isCursorLocked = true;
    
    [ReadOnly] private float timer;

    private void Awake()
    {
        Instance = this;
        characterMovement = GetComponent<CharacterMovement>();
        health = (int)maxHealth;

    }

    private void Start()
    {
        playerStateMachine = characterMovement.StateMachine;
        isCursorLocked = true;
        AlternateCursor();
    }
    

    
    public void TakeDamage(int damage)
    {
        health -= damage;
        OnApplyDamage?.Invoke(this, EventArgs.Empty);
        Debug.Log($"Player took {damage} damage, remaining health: {health}");
    }

    public float GetHealthNormalized()
    {
        return health/(float) maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            AlternateCursor();
        }
    }
    
    private void AlternateCursor()
    {
        isCursorLocked = !isCursorLocked;
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

    public float OnTimerRunningMaskCooldown()
    {
        timer = characterMovement.TimerValue;
        return timer;
    }
    public CharacterMovement GetCharacterMovement()
    {
        return characterMovement;
    }
}
