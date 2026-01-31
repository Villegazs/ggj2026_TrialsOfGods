using UnityEngine;

/// <summary>
/// Script de ejemplo que muestra cómo activar el estado de Wind Mask
/// cuando el jugador recoge un item
/// </summary>
public class WindMaskPickup : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Tag del jugador")]
    public string playerTag = "Player";
    
    [Header("Visual Feedback")]
    [Tooltip("Efecto de partículas al recoger (opcional)")]
    public GameObject pickupEffect;
    
    [Header("Audio")]
    [Tooltip("Sonido al recoger (opcional)")]
    public AudioClip pickupSound;
    
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador
        if (other.CompareTag(playerTag))
        {
            // Activar el estado de Wind Mask a través del evento estático
            StaticEventHandler.RaiseMaskEquipped();
            
            // Feedback visual
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }
            
            // Feedback de audio
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
            
            // Destruir el objeto después de recogerlo
            Destroy(gameObject);
        }
    }
    
    // Método opcional para activar manualmente (por ejemplo, desde un botón UI)
    public void ActivateWindMask()
    {
        StaticEventHandler.RaiseMaskEquipped();
        Debug.Log("Wind Mask activated manually!");
    }
}
