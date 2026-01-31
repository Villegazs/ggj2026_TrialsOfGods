using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    
    private const string TRIGGER_SHOW_PANEL= "Show";
    [Header("Interaction Settings")]
    [SerializeField] protected string interactionPrompt = "Press E to interact";
    [SerializeField] protected float interactionRange = 2f;
    [SerializeField] protected bool oneTimeUse = true;

    [Header("Visual Feedback")]
    [SerializeField] protected Transform interactionPromptPanel;
    [SerializeField] protected GameObject highlightEffect;
    [SerializeField] protected Color highlightColor = Color.yellow;

    protected bool isInteractable = true;
    protected bool isPlayerInRange = false;
    protected MeshRenderer meshRenderer;
    protected Color originalColor;

    protected virtual void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (isPlayerInRange && isInteractable)
        {
            Interact();
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isPlayerInRange = true;
            ShowHighlight();
            EnableInteractionPanel();
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isPlayerInRange = false;
            HideHighlight();
            DisableInteraction();
        }
    }

    protected abstract void Interact();

    protected virtual void ShowHighlight()
    {
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(true);
        }
        else if (meshRenderer != null)
        {
            meshRenderer.material.color = highlightColor;
        }
    }

    protected virtual void HideHighlight()
    {
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(false);
        }
        else if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }
    }

    protected virtual void DisableInteraction()
    {
        isInteractable = false;
        HideHighlight();
        
        if (oneTimeUse)
        {
            gameObject.SetActive(false);
        }
    }
    
    protected virtual void EnableInteractionPanel()
    {
        if (interactionPromptPanel == null) return;
        
        interactionPromptPanel.gameObject.SetActive(true);
        Animator animator = interactionPromptPanel.GetComponent<Animator>();
        animator.SetTrigger(TRIGGER_SHOW_PANEL);
    }
    protected virtual void DisableInteractionPanel()
    {
        interactionPromptPanel.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
