using UnityEngine;
using DG.Tweening;
public class PlayerInventory : MonoBehaviour
{
    [Header("Equipment Points")]
    [SerializeField] private Transform maskEquipPoint;

    [Header("Current Equipment")]
    [SerializeField] private GameObject currentMask;
    private bool hasMask = false;
    
    [Header("DOTween Pickup Animation")]
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private float scaleDuration = 0.5f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;
    [SerializeField] private Ease scaleEase = Ease.InBack;
    

    public bool HasMask => hasMask;

    private void Start()
    {
        if (maskEquipPoint == null)
        {
            Transform head = transform.Find("Head");
            if (head != null)
            {
                maskEquipPoint = head;
            }
            else
            {
                maskEquipPoint = transform;
            }
        }
    }

    public void EquipMask(GameObject maskPrefab)
    {
        if (maskPrefab == null) return;

        if (currentMask != null)
        {
            Destroy(currentMask);
        }

        currentMask = Instantiate(maskPrefab, maskEquipPoint);
        currentMask.transform.localPosition = Vector3.zero;
        currentMask.transform.localRotation = Quaternion.identity;
        hasMask = true;
        PlayPickupTween();
    }

    public void RemoveMask()
    {
        if (currentMask != null)
        {
            Destroy(currentMask);
            currentMask = null;
            hasMask = false;
        }
    }
    
    // ---------------- PICK UP MASK ----------------
    
    
    private void PlayPickupTween()
    {
        currentMask.transform.localScale = Vector3.zero;

        // Crear secuencia: mover -> escalar -> fade (si aplica)
        Sequence seq = DOTween.Sequence();
        seq.Join(currentMask.transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEase));
    }

    public void UnequipMask()
    {
        if (currentMask == null) return;
        UnequipMaskTween();
    }
    private void UnequipMaskTween()
    {

        // Crear secuencia: mover -> escalar -> fade (si aplica)
        Sequence seq = DOTween.Sequence();
        seq.Join(currentMask.transform.DOScale(Vector3.zero, scaleDuration).SetEase(scaleEase));
        
        seq.OnComplete(() =>
        {
            RemoveMask();
        });
    }


}