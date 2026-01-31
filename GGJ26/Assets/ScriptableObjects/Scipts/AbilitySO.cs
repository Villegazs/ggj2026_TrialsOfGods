using UnityEngine;
public abstract class AbilitySO : ScriptableObject
{
    public enum AbilityActivationType
    {
        Hold,
        Instant
    }

    [Header("Activation")]
    public AbilityActivationType activationType = AbilityActivationType.Instant;
    public int priority = 0;

    [Header("Timing")]
    public float cooldown = 0.5f;
    public float inputBufferTime = 0.15f;
    
    public abstract bool WantsToActivate(CharacterMovement ctx);
    public virtual bool CanUse(CharacterMovement ctx) => true;
    public abstract void Execute(CharacterMovement ctx);
    public virtual void Tick(CharacterMovement ctx) { }
    public virtual void End(CharacterMovement ctx) { }
}