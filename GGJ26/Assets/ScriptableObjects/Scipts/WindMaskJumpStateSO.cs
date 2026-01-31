using UnityEngine;

[CreateAssetMenu(fileName = "New Wind Mask Jump State", menuName = "Movement States/Wind Mask Jump", order = 1)]
public class WindMaskJumpStateSO : MovementStateSO
{
    [Header("Wind Mask Jump Settings")]
    public MovementSettingsSO windMaskJumpMovement;
    public float windMaskJumpDuration = 2f;
    public float extraJumpForce = 1.5f; // Multiplicador de fuerza de salto adicional
    
    [System.NonSerialized]
    private float windMaskJumpTimer;

    public override void Enter(CharacterMovement ctx)
    {
        Debug.Log("Entering Wind Mask Jump State");
        
        // Apply wind mask jump movement settings if configured
        if (windMaskJumpMovement != null)
        {
            ctx.ApplyMovementSettings(windMaskJumpMovement);
        }
        
        UpdateAbilities(ctx);
        windMaskJumpTimer = windMaskJumpDuration;
        
        // Apply extra jump force for a powered jump
        float boostedJumpForce = ctx.jumpForce * extraJumpForce;
        ctx.SetVerticalVelocity(boostedJumpForce);
        ctx.ConsumeJumpBuffer();
        
        // Raise jump event for audio/visual feedback
        StaticEventHandler.RaiseJump();
    }

    public override void Tick(CharacterMovement ctx)
    {
        // Handle horizontal movement with enhanced air control
        ctx.HandleHorizontalMovement(ctx.AirControl);
        
        // Apply gravity (can be customized for floaty feel)
        ctx.AddVerticalVelocity(ctx.Gravity * Time.deltaTime);
        
        // Update timer
        windMaskJumpTimer -= Time.deltaTime;
        
        // Check if timer expired or landed
        if (windMaskJumpTimer <= 0f)
        {
            // Return to appropriate state based on grounded status
            if (ctx.Controller.isGrounded)
            {
                ctx.StateMachine.SwitchState(ctx.groundedStateSo, ctx);
            }
            else
            {
                ctx.StateMachine.SwitchState(ctx.airStateSo, ctx);
            }
        }
        else if (ctx.Controller.isGrounded)
        {
            // If landed before timer expires, go to grounded state
            ctx.SetVerticalVelocity(ctx.GroundedStickForce);
            ctx.StateMachine.SwitchState(ctx.groundedStateSo, ctx);
        }
    }

    public override void Exit(CharacterMovement ctx)
    {
        Debug.Log("Exiting Wind Mask Jump State");
        ctx.ResetSpeedMultiplier();
    }
}
