using UnityEngine;

public class Robot_Audio : MonoBehaviour
{
    [SerializeField] private AudioClip _activationClip;
    [SerializeField] private AudioClip _deactivationClip;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _startPointingPlayer;
    [SerializeField] private AudioClip _stopPointingPlayer;

    public AudioClip HurtClip => _hurtClip;

    public void PlayActivationClip() => AudioHandler.Instance.PlaySfx(_activationClip);
    public void PlayDeactivationClip() => AudioHandler.Instance.PlaySfx(_deactivationClip);
    public void PlayShootClip() => AudioHandler.Instance.PlaySfx(_shootClip);
    public void PlayDeathClip() => AudioHandler.Instance.PlaySfx(_deathClip);
    public void PlayStartPointingClip() => AudioHandler.Instance.PlaySfx(_startPointingPlayer);
    public void PlayStopPointingClip() => AudioHandler.Instance.PlaySfx(_stopPointingPlayer);
}
