using UnityEngine;

public abstract class MovementStateSO : ScriptableObject
{
    [Header("Abilities Allowed In This State")]
    public AbilitySO[] abilities;

    public abstract void Enter(CharacterMovement ctx);
    public abstract void Tick(CharacterMovement ctx);
    public abstract void Exit(CharacterMovement ctx);

    protected void UpdateAbilities(CharacterMovement ctx)
    {
        ctx.SetAllowedAbilities(abilities);
    }
}