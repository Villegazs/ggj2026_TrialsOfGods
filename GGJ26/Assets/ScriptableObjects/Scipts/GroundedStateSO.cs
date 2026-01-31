using UnityEngine;

[CreateAssetMenu(menuName = "Movement States/Grounded")]
public class GroundedStateSO : MovementStateSO
{
    public MovementSettingsSO groundedMovement;

    public override void Enter(CharacterMovement ctx)
    {
        ctx.ApplyMovementSettings(groundedMovement);
        UpdateAbilities(ctx);
      

        if (ctx.Velocity.y < 0)
            ctx.SetVerticalVelocity(ctx.GroundedStickForce);
    }

    public override void Tick(CharacterMovement ctx)
    {
       
        ctx.HandleHorizontalMovement(1f);

        if (ctx.CanGroundJump())
        {
            ctx.SetVerticalVelocity(ctx.jumpForce);
            ctx.ConsumeJumpBuffer();
            ctx.StateMachine.SwitchState(ctx.airStateSo, ctx);
            return;
        }

        if (!ctx.Controller.isGrounded)
            ctx.StateMachine.SwitchState(ctx.airStateSo, ctx);
    }

    public override void Exit(CharacterMovement ctx)
    {
        ctx.ResetSpeedMultiplier();
    }
}