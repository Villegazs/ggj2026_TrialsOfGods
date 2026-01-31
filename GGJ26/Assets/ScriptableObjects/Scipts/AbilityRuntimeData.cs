public class AbilityRuntimeData
{
    public float cooldownTimer;
    public float inputBufferTimer;

    public bool IsOnCooldown => cooldownTimer > 0f;
    public bool HasBufferedInput => inputBufferTimer > 0f;
}