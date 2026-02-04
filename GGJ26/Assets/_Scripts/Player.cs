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
    [SerializeField] private Animator animator;
    
    private CharacterMovement characterMovement;
    private PlayerStateMachine playerStateMachine;
    private int health;
    
    [ReadOnly] private float timer;

    private void Awake()
    {
        Instance = this;
        characterMovement = GetComponent<CharacterMovement>();
        health = (int)maxHealth;
        animator.enabled = true;
    }

    private void Start()
    {
        playerStateMachine = characterMovement.StateMachine;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    

    
    public void TakeDamage(int damage)
    {
        health -= damage;
        OnApplyDamage?.Invoke(this, EventArgs.Empty);
        Debug.Log($"Player took {damage} damage, remaining health: {health}");
        if (IsDead())
        {
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
