using UnityEngine;
using UnityEngine.Events;
using System;

public class PressurePlate : MonoBehaviour
{
    public event EventHandler OnActivated;
    public event EventHandler OnDeactivated;
    
    [Header("Activation Settings")]
    [SerializeField] private LayerMask activationLayer;
    [SerializeField] private bool stayActivated = false;
    [SerializeField] private float activationDuration = 5f;

    [Header("Visual Feedback")]
    [SerializeField] private float pressedHeight = 0.1f;
    [SerializeField] private float pressSpeed = 5f;
    [SerializeField] private Material activatedMaterial;
    [SerializeField] private GameObject activationEffect;

    [Header("Events")]
    //public UnityEvent OnActivated;
    //public UnityEvent OnDeactivated;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    private bool isPermanentlyActivated = false;
    private float activationTimer = 0f;
    private int objectsOnPlate = 0;
    private MeshRenderer meshRenderer;
    private Material originalMaterial;

    private void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition - new Vector3(0, pressedHeight, 0);
        
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
    }

    private void Update()
    {
        // Mover la placa visualmente
        Vector3 targetPosition = (isPressed || isPermanentlyActivated) ? pressedPosition : originalPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, pressSpeed * Time.deltaTime);

        // Timer de desactivación
        if (!stayActivated && isPressed && objectsOnPlate == 0)
        {
            activationTimer -= Time.deltaTime;
            if (activationTimer <= 0f)
            {
                Deactivate();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPermanentlyActivated) return;

        if (((1 << other.gameObject.layer) & activationLayer) != 0)
        {
            objectsOnPlate++;
            
            if (!isPressed)
            {
                Activate();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPermanentlyActivated || stayActivated) return;

        if (((1 << other.gameObject.layer) & activationLayer) != 0)
        {
            objectsOnPlate--;
            
            if (objectsOnPlate <= 0)
            {
                objectsOnPlate = 0;
                activationTimer = activationDuration;
            }
        }
    }

    private void Activate()
    {
        isPressed = true;
        activationTimer = activationDuration;

        if (activatedMaterial != null && meshRenderer != null)
        {
            meshRenderer.material = activatedMaterial;
        }

        if (activationEffect != null)
        {
            Instantiate(activationEffect, transform.position, Quaternion.identity);
        }

        OnActivated?.Invoke(this, EventArgs.Empty);

        if (stayActivated)
        {
            isPermanentlyActivated = true;
        }
    }

    private void Deactivate()
    {
        isPressed = false;

        if (meshRenderer != null && originalMaterial != null)
        {
            meshRenderer.material = originalMaterial;
        }

        OnDeactivated?.Invoke(this, EventArgs.Empty);
    }
}
