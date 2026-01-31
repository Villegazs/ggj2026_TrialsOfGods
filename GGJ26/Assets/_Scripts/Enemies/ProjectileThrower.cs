using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    
    [Header("Timing")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float initialDelay = 0f;
    [SerializeField] private bool autoFire = true;
    
    [Header("Warning")]
    [SerializeField] private float warningTime = 0.5f;
    [SerializeField] private GameObject warningEffect;
    
    private float fireTimer;
    
    private void Start()
    {
        fireTimer = initialDelay;
    }
    
    private void Update()
    {
        if (!autoFire) return;
        
        fireTimer -= Time.deltaTime;
        
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireRate;
        }
    }
    
    public void Fire()
    {
        if (warningTime > 0f)
        {
            Invoke(nameof(SpawnProjectile), warningTime);
            ShowWarning();
        }
        else
        {
            SpawnProjectile();
        }
    }
    
    private void SpawnProjectile()
    {
        Vector3 throwDirection = spawnPoint.forward;
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>()?.Initialize(throwDirection);
    }
    
    private void ShowWarning()
    {
        if (warningEffect != null)
        {
            GameObject warning = Instantiate(warningEffect, spawnPoint.position, spawnPoint.rotation);
            Destroy(warning, warningTime);
        }
    }
}