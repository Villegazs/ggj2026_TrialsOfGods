using System;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour , IDamageable
{
    private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Player took {damage} damage, remaining health: {health}");
    }

    public int GetHealth()
    {
        return health;
    }
}
