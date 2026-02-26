using System.Collections;
using UnityEngine;

public class PickableItemObject : MonoBehaviour, IPickable
{
    [Header("ItemData")]
    [SerializeField] private PickableItemData _scriptableObjectData;
    [Space(5)]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _source;

    private WaitForSeconds _pickUpDelay = new(0.1f);

    private void Awake()
    {
        _spriteRenderer.sprite = _scriptableObjectData.ItemSprite;
        _source.clip = _scriptableObjectData.ItemInteractionSound;
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
        _source.Play();
        yield return _pickUpDelay;
        Debug.Log(_scriptableObjectData.Name + " +" + _scriptableObjectData.ScoreValue);
        gameObject.SetActive(false);
    }

    public void PickUp()
    {
        StartCoroutine(ExecutePickUpAction());
    }
}
