using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Sprint")]
public class SprintAbilitySO : AbilitySO
{
    [Space]    [Space]    [Space]
    [Header("Ability Effects")]
    public float speedMultiplier = 1.5f;

    /// <summary>
    /// Tells the character Movement that the Ability has been called, and waits for approval to execute
    /// </summary>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public override bool WantsToActivate(CharacterMovement ctx)
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public override void Execute(CharacterMovement ctx)
    {
        ctx.SetSpeedMultiplier(speedMultiplier);
    }
    public override void End(CharacterMovement ctx)
    {
        ctx.ResetSpeedMultiplier();
    }
}