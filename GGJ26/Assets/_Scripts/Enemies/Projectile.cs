using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask damageLayer;
    
    [Header("Effects")]
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private bool destroyOnImpact = true;
    
    private Vector3 direction;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
    
    public void Initialize(Vector3 direction)
    {
        this.direction = direction.normalized;
        transform.forward = this.direction;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & damageLayer) != 0)
        {
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Debug.Log("Damaged player");
            
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
}
