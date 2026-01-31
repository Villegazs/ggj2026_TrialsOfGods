using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KnockbackHandler : MonoBehaviour
{
    [Header("Knockback Settings")]
    public Vector3 currentKnockback = Vector3.zero;
    public float decay = 5f; // how quickly knockback fades

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (currentKnockback.sqrMagnitude > 0.01f)
        {
            // Apply knockback to character
            controller.Move(currentKnockback * Time.deltaTime);

            // Gradually decay knockback
            currentKnockback = Vector3.Lerp(currentKnockback, Vector3.zero, decay * Time.deltaTime);
        }
    }

    /// <summary>
    /// Apply a knockback to the character.
    /// </summary>
    /// <param name="direction">World-space direction</param>
    /// <param name="horizontalForce">X/Z force</param>
    /// <param name="verticalForce">Y force</param>
    public void ApplyKnockback(Vector3 direction, float horizontalForce, float verticalForce)
    {
        Vector3 knock = direction.normalized * horizontalForce;
        knock.y = verticalForce;
        currentKnockback += knock;
    }
}