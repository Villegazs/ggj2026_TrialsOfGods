using UnityEngine;

[CreateAssetMenu(menuName = "Movement States/Air")]
public class AirStateSO : MovementStateSO
{
    public MovementSettingsSO aerealMovement;
    public override void Enter(CharacterMovement ctx)
    {
        ctx.ApplyMovementSettings(aerealMovement);
        UpdateAbilities(ctx); // different ability set
    }

    public override void Tick(CharacterMovement ctx)
    {
        ctx.HandleHorizontalMovement(ctx.AirControl);

        // -------- DOUBLE JUMP --------
        if (ctx.CanAirJump())
        {
            ctx.SetVerticalVelocity(ctx.jumpForce);
            ctx.ConsumeAirJump();
            StaticEventHandler.RaiseJump();
            ctx.ConsumeJumpBuffer();
            return;
        }

        // Gravity
        ctx.AddVerticalVelocity(ctx.Gravity * Time.deltaTime);

        if (ctx.Controller.isGrounded)
        {
            StaticEventHandler.RaiseLand();
            ctx.SetVerticalVelocity(ctx.GroundedStickForce);
            ctx.StateMachine.SwitchState(ctx.groundedStateSo, ctx);
        }
    }

    public override void Exit(CharacterMovement ctx) { }
}