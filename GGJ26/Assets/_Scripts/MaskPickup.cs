using UnityEngine;

public enum Masks
{
    Wind,
    Light,
    None
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
            PlayPickupEffects();
            if (_masks == Masks.Wind)
            {
                CharacterMovement _PlayerCharacterController = playerInventory.gameObject.GetComponent<CharacterMovement>();
                _PlayerCharacterController.SetMaskPickup(maskPrefab);
                _PlayerCharacterController.EquipMask(_masks);
                StaticEventHandler.RaiseMaskEquipped();
            }
            else
            {
                //Todo Otra mascara
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