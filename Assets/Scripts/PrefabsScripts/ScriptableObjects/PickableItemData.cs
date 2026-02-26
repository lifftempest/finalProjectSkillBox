using UnityEngine;

[CreateAssetMenu(fileName = "PickableItemData", menuName = "Scriptable Objects/PickableItemData")]
public class PickableItemData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private AudioClip _itemInteractiveSound;
    [SerializeField] private int _scoreValue;

    public string Name => _name;
    public int ScoreValue => _scoreValue;
    public Sprite ItemSprite => _itemSprite;
    public AudioClip ItemInteractionSound => _itemInteractiveSound;
}
