using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Equipment Points")]
    [SerializeField] private Transform maskEquipPoint;

    [Header("Current Equipment")]
    private GameObject currentMask;
    private bool hasMask = false;

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
}