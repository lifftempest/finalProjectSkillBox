using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio_Handler : MonoBehaviour
{
    [Header("MovementSounds")]
    [SerializeField] private AudioClip _jumpStartClip;
    [SerializeField] private AudioClip _landingClip;
    [SerializeField] private AudioClip[] _runClips;
    [Header("FightSounds")]
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private AudioClip _emptyMagazineClip;
    [SerializeField] private AudioClip[] _hurtClips;
    [SerializeField] private AudioClip _deathClip;

    private Queue<AudioClip> _footstepsClips = new();
    private Queue<AudioClip> _takeDamageClips = new();

    public AudioClip EmptyMagazineClickClip => _emptyMagazineClip;
    public AudioClip FireClip => _fireClip;

    private void Awake()
    {
        foreach (var item in _runClips)
        {
            _footstepsClips.Enqueue(item);
        }
        foreach (var item in _hurtClips)
        {
            _takeDamageClips.Enqueue(item);
        }
    }

    public void PlayFootstep()
    {
        var clip = _footstepsClips.Dequeue();
        AudioHandler.Instance.PlaySfx(clip);
        _footstepsClips.Enqueue(clip);
    }

    public void PlayHurtClip()
    {
        var clip = _takeDamageClips.Dequeue();
        AudioHandler.Instance.PlaySfx(clip);
        _takeDamageClips.Enqueue(clip);
    }

    public void PlayJumpStartClip() => AudioHandler.Instance.PlaySfx(_jumpStartClip);
    public void PlayerLandingClip() => AudioHandler.Instance.PlaySfx(_landingClip);
    public void PlayDeathClip() => AudioHandler.Instance.PlaySfx(_deathClip);
}
