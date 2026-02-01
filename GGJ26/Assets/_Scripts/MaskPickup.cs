using UnityEngine;
using DG.Tweening;
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
    
    [Header("Tween Settings")]
    [SerializeField] private float tweenDuration = 0.6f;
    [SerializeField] private float moveUpDistance = 1f;
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
            PlayPickupTween();
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
    
    private void PlayPickupTween()
    {
        // Construye secuencia: mover hacia arriba + escalar a 0 (+ fade si es sprite)
        Sequence seq = DOTween.Sequence();

        Vector3 targetPos = transform.position + Vector3.up * moveUpDistance;
        seq.Append(transform.DOMove(targetPos, tweenDuration).SetEase(Ease.OutCubic));
        seq.Join(transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));

        // Si tiene SpriteRenderer, también hacer fade
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Asegurar material con color
            seq.Join(sr.DOFade(0f, tweenDuration));
        }
        else
        {
            // Si es UI (CanvasGroup) se podría añadir aquí
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                seq.Join(cg.DOFade(0f, tweenDuration));
            }
        }

        seq.OnComplete(() =>
        {
            OnPickup(); // comportamiento heredado
            Destroy(gameObject);
        });
    }
}