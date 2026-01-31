using UnityEngine;

enum Masks
{
    Wind,
    Light
}
public class MaskPickup : Interactable
{
    [Header("Mask Settings")]
    [SerializeField] private GameObject maskPrefab;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private Masks _masks;
    protected override void Interact()
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        
        if (playerInventory != null)
        {
            playerInventory.EquipMask(maskPrefab);
            PlayPickupEffects();
            if (_masks == Masks.Wind)
            {
                StaticEventHandler.RaiseMaskEquipped();
            }
            else
            {
                
            }
            DisableInteraction();
        }
    }

    private void PlayPickupEffects()
    {
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }
}