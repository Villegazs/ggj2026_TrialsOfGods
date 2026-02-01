using UnityEngine;

public class HandleAnimations : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private float walkThreshold = 0.1f;

    private Animator animator;

    private bool wasGrounded;

    // Animator hashes
    private static readonly int IsWalkingHash  = Animator.StringToHash("IsWalking");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash       = Animator.StringToHash("Jump");

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (characterMovement == null)
            characterMovement = GetComponentInParent<CharacterMovement>();
    }

    void Update()
    {
        if (characterMovement == null)
            return;

        UpdateWalking();
        UpdateGrounded();
        UpdateJumpTrigger();

        wasGrounded = characterMovement.Controller.isGrounded;
    }

    // ---------------- WALK ----------------
    void UpdateWalking()
    {
        bool isWalking =
            characterMovement.HorizontalVelocity.magnitude > walkThreshold;

        animator.SetBool(IsWalkingHash, isWalking);
    }

    // ---------------- GROUNDED ----------------
    void UpdateGrounded()
    {
        bool isGrounded = characterMovement.Controller.isGrounded;
        animator.SetBool(IsGroundedHash, isGrounded);
    }

    // ---------------- JUMP (NOT FALL) ----------------
    void UpdateJumpTrigger()
    {
        bool isGrounded = characterMovement.Controller.isGrounded;

        bool jumpStarted =
            wasGrounded &&
            !isGrounded &&
            characterMovement.JumpPressedThisFrame &&
            characterMovement.Velocity.y > 0f;

        if (jumpStarted)
        {
            animator.SetTrigger(JumpHash);
        }
    }
}