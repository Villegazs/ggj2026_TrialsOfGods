using UnityEngine;

[CreateAssetMenu(fileName = "New Wind Mask State", menuName = "Movement States/Using Wind Mask", order = 0)]
public class UsingWindMaskStateSO : MovementStateSO
{
    [Header("Wind Mask Settings")]
    public MovementSettingsSO windMaskMovement;
    public float windMaskDuration = 3f;
    
    [System.NonSerialized]
    private float windMaskTimer;

    public override void Enter(CharacterMovement ctx)
    {
        Debug.Log("Entering Wind Mask State");
        
        // Apply wind mask movement settings if configured
        if (windMaskMovement != null)
        {
            ctx.ApplyMovementSettings(windMaskMovement);
        }
        
        UpdateAbilities(ctx);
        windMaskTimer = windMaskDuration;
    }

    public override void Tick(CharacterMovement ctx)
    {
        // Check if player wants to jump while in wind mask state
        if (ctx.HasBufferedJump() && ctx.windMaskJumpStateSo != null)
        {
            // Transition to Wind Mask Jump state (special jump only available from this state)
            ctx.StateMachine.SwitchState(ctx.windMaskJumpStateSo, ctx);
            return;
        }
        
        // Handle horizontal movement with wind mask active
        ctx.HandleHorizontalMovement(ctx.Controller.isGrounded ? 1f : ctx.AirControl);
        
        // Apply gravity if in air
        if (!ctx.Controller.isGrounded)
        {
            ctx.AddVerticalVelocity(ctx.Gravity * Time.deltaTime);
        }
        else
        {
            if (ctx.Velocity.y < 0)
                ctx.SetVerticalVelocity(ctx.GroundedStickForce);
        }
        
        // Update timer and exit state when duration ends
        windMaskTimer -= Time.deltaTime;
        if (windMaskTimer <= 0f)
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
    }

    public override void Exit(CharacterMovement ctx)
    {
        Debug.Log("Exiting Wind Mask State");
        ctx.ResetSpeedMultiplier();
    }
}
