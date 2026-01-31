using System;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour , IDamageable
{
    private int health = 100;
    private bool isCursorLocked = true;

    private void Start()
    {
        isCursorLocked = true;
        AlternateCursor();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Player took {damage} damage, remaining health: {health}");
    }

    public int GetHealth()
    {
        return health;
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
