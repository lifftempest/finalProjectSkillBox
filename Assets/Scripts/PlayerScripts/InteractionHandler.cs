using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private Transform _interactionOrigin;
    [SerializeField] private float _interactionRadius;

    public void TryInteract(bool interactInput)
    {
        if (interactInput)
        {
            print("Tried to Interact");
            var hits = Physics2D.CircleCastAll(_interactionOrigin.position, _interactionRadius, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var item))
                {
                    item.Interact();
                    print("interacted");
                }
            }
        }
    }
}
