using UnityEngine;
using System;

public class CompleteLevel : MonoBehaviour
{
    public event EventHandler OnLevelCompleted;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Level completed");
        if (other.TryGetComponent(out Player player))
        {
            if(player.IsDead()) return;
            OnLevelCompleted?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Player not found");
        }
    }
}
