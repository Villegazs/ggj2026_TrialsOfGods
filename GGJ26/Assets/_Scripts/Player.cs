using System;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour , IDamageable
{
    public event EventHandler OnApplyDamage;
    
    public static Player Instance { get; private set; }
    [SerializeField] private float maxHealth = 5;
    
    private int health;
    
    private bool isCursorLocked = true;

    private void Awake()
    {
        Instance = this;
        health = (int)maxHealth;
    }

    private void Start()
    {
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
}
