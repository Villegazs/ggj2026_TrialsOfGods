using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbilitySO : AbilitySO
{
    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;

    float timer;
    Vector3 dashDirection;

    public override bool WantsToActivate(CharacterMovement ctx)
    {
        // Still OK: this is *activation intent*, not movement input
        return Input.GetKeyDown(KeyCode.LeftControl);
    }

    public override void Execute(CharacterMovement ctx)
    {
        timer = dashDuration;

        // Use cached input
        if (ctx.MoveInputWorld.sqrMagnitude > 0.01f)
            dashDirection = ctx.MoveInputWorld.normalized;
        else
            dashDirection = ctx.transform.forward;

        ctx.HorizontalVelocity = dashDirection * dashSpeed;
    }

    public override void Tick(CharacterMovement ctx)
    {
        timer -= Time.deltaTime;

        ctx.HorizontalVelocity = dashDirection * dashSpeed;

        if (timer <= 0f)
            ctx.EndAbility(this);
    }
}