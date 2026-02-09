using UnityEngine;
using System;

public class CompleteLevel : MonoBehaviour
{
    public event EventHandler OnLevelCompleted;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Level completed");
        if (other.gameObject.CompareTag("Player"))
        {
            OnLevelCompleted?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Player not found");
        }
    }
}
