using UnityEngine;

public class Trader_Audio : MonoBehaviour
{
    [SerializeField] private AudioClip _openShopClip;
    [SerializeField] private AudioClip _closeShopClip;
    [SerializeField] private AudioClip _greetingsClip;
    [SerializeField] private AudioClip _interactClip;

    public void PlayOpenShopClip() => AudioHandler.Instance.PlaySfx(_openShopClip);
    public void PlayCloseShopClip() => AudioHandler.Instance.PlaySfx(_closeShopClip);
    public void PlayGreetClip() => AudioHandler.Instance.PlaySfx(_greetingsClip);
    public void PlayInteractionClip() => AudioHandler.Instance.PlaySfx(_interactClip);
}
