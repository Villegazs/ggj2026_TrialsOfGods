using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float damageInterval = 1f;
    [SerializeField] protected LayerMask playerLayer;
    
    [Header("Knockback")]
    [SerializeField] protected bool applyKnockback = true;
    [SerializeField] protected float knockbackForce = 10f;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            DamagePlayer(other);
        }
    }
    
    protected bool IsPlayer(Collider other)
    {
        return ((1 << other.gameObject.layer) & playerLayer) != 0;
    }
    
    protected virtual void DamagePlayer(Collider player)
    {
        // Implementar lógica de daño
        // player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        
        Debug.Log("Damaged player");
        if (applyKnockback)
        {
            ApplyKnockback(player);
        }
    }
    
    protected virtual void ApplyKnockback(Collider player)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // player.GetComponent<CharacterMovement>()?.AddVerticalVelocity(knockbackForce);
    }
}