using System.Collections.Generic;
using UnityEngine;

public class Bum_Audio : MonoBehaviour
{
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip _getUpClip;
    [SerializeField] private AudioClip _sitDownClip;
    [SerializeField] private AudioClip _attackMissClip;
    [SerializeField] private AudioClip[] _bumRunClips;
    [SerializeField] private AudioClip[] _bumHurtClips;

    private Queue<AudioClip> _runClipsQueue = new();
    private Queue<AudioClip> _hurtClipsQueue = new();

    public AudioClip HitClip => _hitClip;
    public AudioClip AttackMissClip => _attackMissClip;

    private void Awake()
    {
        foreach (var clip in _bumRunClips)
        {
            _runClipsQueue.Enqueue(clip);
        }
        foreach (var item in _bumHurtClips)
        {
            _hurtClipsQueue.Enqueue(item);
        }
    }

    public void PlayDeathClip() => AudioHandler.Instance.PlaySfx(_deathClip);
    public void PlayGetUpClip() => AudioHandler.Instance.PlaySfx(_getUpClip);
    public void PlaySitDownClip() => AudioHandler.Instance.PlaySfx(_sitDownClip);

    public void PlayFootstepClip()
    {
        var clip = _runClipsQueue.Dequeue();
        AudioHandler.Instance.PlaySfx(clip);
        _runClipsQueue.Enqueue(clip);
    }
    public void PlayHurtClip()
    {
        var clip = _hurtClipsQueue.Dequeue();
        AudioHandler.Instance.PlaySfx(clip);
        _hurtClipsQueue.Enqueue(clip);
    }
}
