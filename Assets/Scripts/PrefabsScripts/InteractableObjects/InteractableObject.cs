using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    public string InteractionPrompt => $"Нажмите {InputVariables.INTERACTION_BUTTON}, чтобы {_interactionPrompt}";

    protected bool _isInteractable = true;
    protected bool _isInRadius;

    private void Awake()
    {
        _audioSource.clip = _clip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_isInteractable)
            {
                _isInRadius = true;
                Debug.Log(InteractionPrompt);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_isInteractable)
            {
                _isInRadius = false;
                Debug.Log("Lost interactiveObj");
            }
        }
    }

    public void Interact()
    {
        if (_isInRadius)
        {
            _audioSource.Play();
            ExecuteInteraction();
        }
    }
    /// <summary>
    /// Необходимо определить в реализации останется ли объект интерактивным или нет
    /// </summary>
    protected abstract void ExecuteInteraction();
}
