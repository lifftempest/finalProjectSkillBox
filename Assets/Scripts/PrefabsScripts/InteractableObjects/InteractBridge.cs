using UnityEngine;

public class InteractBridge : MonoBehaviour, IInteractable
{
    private IInteractable _parentInteraction;

    private void Awake()
    {
        _parentInteraction = transform.parent.GetComponent<IInteractable>();
    }

    public string InteractionPrompt => _parentInteraction.InteractionPrompt;

    public void Interact()
    {
        _parentInteraction.Interact();
    }
}
