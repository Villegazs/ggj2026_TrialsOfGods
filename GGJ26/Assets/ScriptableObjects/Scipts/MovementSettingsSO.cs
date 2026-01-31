using UnityEngine;
/// <summary>
/// This is a SO that handles the values of the movement, we use this in order to not have repetition or overlapping of movement settings
/// </summary>
[CreateAssetMenu(menuName = "Movement/Movement Settings")]
public class MovementSettingsSO : ScriptableObject
{
    public float moveSpeed = 6f;
    public float acceleration = 10f;
    public float gravity = -20f;
    public float airControl = 0.4f;
}