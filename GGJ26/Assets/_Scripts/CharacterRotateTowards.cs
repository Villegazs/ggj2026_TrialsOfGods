using UnityEngine;

public class CharacterRotateTowards : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float rotationSpeed = 10f;

    private void Update()
    {
        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;

        // If we're not moving, don't rotate
        if (horizontalVelocity.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}