using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Scriptable Objects/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    public Action OnSettingsChanged;

    [Range(0, 1)]
    [SerializeField] private float _musicVolume = 1f;
    [Range(0, 1)]
    [SerializeField] private float _sfxVolume = 1f;

    public float MusicVolume
    {
        get => _musicVolume;
        set 
        {
            _musicVolume = value;
            OnSettingsChanged?.Invoke();
        }
    }
    public float SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            OnSettingsChanged?.Invoke();
        }
    }
}
