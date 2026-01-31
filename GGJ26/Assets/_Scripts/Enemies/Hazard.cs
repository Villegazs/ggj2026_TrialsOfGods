using UnityEngine;

public class Hazard : MonoBehaviour
{
    
    [Header("Damage")]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float damageInterval = 1f;
    [SerializeField] protected LayerMask playerLayer;
    
    [Header("Knockback")]
    [SerializeField] protected bool applyKnockback = true;
    [SerializeField] float knockbackX = 5f;
    [SerializeField] float knockbackY = 3f;

    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            DamagePlayer(other);
        }
    }
    
    protected bool IsPlayer(Collider other)
    {
        other.TryGetComponent(out Player player);
        return player != null;
    }
    
    protected virtual void DamagePlayer(Collider player)
    {
        // Implementar lógica de daño
        // player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        if (player.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
        
        
        if (applyKnockback)
        {
            ApplyKnockback(player);
        }
    }
    
    protected virtual void ApplyKnockback(Collider player)
    {
        KnockbackHandler knockback = player.GetComponent<KnockbackHandler>();
        if (knockback != null)
        {
            Vector3 knockDir = (player.transform.position - transform.position).normalized;
            knockback.ApplyKnockback(knockDir, knockbackX, knockbackY);
        }
    }
}