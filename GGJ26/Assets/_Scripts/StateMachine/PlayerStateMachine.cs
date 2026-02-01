public class PlayerStateMachine
{
    public MovementStateSO CurrentState { get; private set; }

    public void Initialize(MovementStateSO start, CharacterMovement ctx)
    {
        CurrentState = start;
        CurrentState.Enter(ctx);
    }

    public void SwitchState(MovementStateSO next, CharacterMovement ctx)
    {
        if (next == CurrentState || next == null)
            return;

        ctx.ResetAbilityData();
        

        CurrentState.Exit(ctx); 
        if (CurrentState == ctx.usingWindMaskStateSo)
            ctx.StartMaskCooldown();
        CurrentState = next;
        CurrentState.Enter(ctx);
    }

    public void Tick(CharacterMovement ctx)
    {
        CurrentState?.Tick(ctx);
    }
}