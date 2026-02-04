using System;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    
    private const string TRIGGER_SHOW_PANEL= "Popup";
    [Header("Interaction Settings")]
    [SerializeField] protected string interactionPrompt = "E";
    [SerializeField] protected float interactionRange = 2f;
    [SerializeField] protected bool oneTimeUse = true;
    [SerializeField] protected GameInput.Binding binding;

    [Header("Visual Feedback")]
    [SerializeField] protected Transform interactionPromptPanel;
    [SerializeField] protected TextMeshProUGUI keyInteractText;
    [SerializeField] protected GameObject highlightEffect;
    [SerializeField] protected Color highlightColor = Color.yellow;

    protected bool isInteractable = true;
    protected bool isPlayerInRange = false;
    protected MeshRenderer meshRenderer;
    protected Color originalColor;

    protected virtual void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
           // originalColor = meshRenderer.material.color;
        }
        SetKeyInteractText();
        interactionPromptPanel.gameObject.SetActive(false);
    }
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (CanInteract())
        {
            Interact();
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (CanInteract())
        {
            Interact();
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isInteractable = true;
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
        
        DisableInteractionPanel();
        
       /* if (oneTimeUse)
        {
            gameObject.SetActive(false);
        }*/
    }

    protected virtual void OnPickup()
    {
        DisableInteraction();
        gameObject.SetActive(false);
    }
    
    protected virtual void EnableInteractionPanel()
    {
        if (interactionPromptPanel == null) return;
        
        interactionPromptPanel.gameObject.SetActive(true);
        Animator animator = interactionPromptPanel.GetComponent<Animator>();
        animator.SetTrigger(TRIGGER_SHOW_PANEL);
    }

    private void SetKeyInteractText()
    {
        keyInteractText.text = GameInput.Instance.GetBindingText(binding);
    }

    protected virtual void DisableInteractionPanel()
    {
        interactionPromptPanel.gameObject.SetActive(false);
    }

    protected virtual bool CanInteract()
    {
        Debug.Log($"Interaction prompt: {GameInput.Instance.GetBindingText(binding)} - {interactionPrompt}");
        
        bool canInteract =isPlayerInRange && isInteractable && (GameInput.Instance.GetBindingText(binding) == interactionPrompt);
        Debug.Log($"Can interact: {canInteract}");
        return canInteract;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
    
    private void OnDestroy()
    {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction -= GameInput_OnInteractAlternateAction;
    }
}
