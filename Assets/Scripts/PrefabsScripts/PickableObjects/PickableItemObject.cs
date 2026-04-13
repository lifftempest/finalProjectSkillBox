using System.Collections;
using UnityEngine;

public class PickableItemObject : MonoBehaviour, IPickable
{
    [Header("ItemData")]
    [SerializeField] private PickableItemData _scriptableObjectData;
    [Space(5)]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ScoreComponent _scoreComponent;

    private WaitForSeconds _pickUpDelay = new(0.1f);
    private int _scoreValue;

    private void Awake()
    {
        _spriteRenderer.sprite = _scriptableObjectData.ItemSprite;
        _scoreValue = _scriptableObjectData.ScoreValue;
        _scoreComponent.SetScoreValue(_scoreValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PickUp();
        }
    }

    private IEnumerator ExecutePickUpAction()
    {
        AudioHandler.Instance.PlaySfx(_scriptableObjectData.ItemInteractionSound);
        ScoreHandler.Instance.AddScore(_scoreComponent.ScoreValue);
        yield return _pickUpDelay;
        gameObject.SetActive(false);
    }

    public void PickUp()
    {
        StartCoroutine(ExecutePickUpAction());
    }
}
