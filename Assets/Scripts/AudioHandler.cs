using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Header("AudioSettingsSO")]
    [SerializeField] private AudioSettings _audioSettings;
    [Space(5), SerializeField] private AudioSource _musicSource;
    [Header("AudioSorcePool Settings")]
    [SerializeField] private int _poolSize;
    [SerializeField] private GameObject _sfxAudioSourcePrefab;

    private Queue<AudioSource> _sfxPool = new();
    private List<AudioSource> _activeSfxSources = new();

    public static AudioHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ApplyMusicVolume();

        InitializeSfxPool();

        _audioSettings.OnSettingsChanged += OnAudioSettingsChanged;
    }

    private void OnDestroy()
    {
        _audioSettings.OnSettingsChanged -= OnAudioSettingsChanged;
    }

    private void InitializeSfxPool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewSfxSource();
        }
    }

    private void CreateNewSfxSource()
    {
        GameObject sourceObject = Instantiate(_sfxAudioSourcePrefab, transform);
        sourceObject.SetActive(false);
        var source = sourceObject.GetComponent<AudioSource>();
        _sfxPool.Enqueue(source);
    }

    public void PlaySfx(AudioClip clip)
    {
        var source = _sfxPool.Dequeue();
        source.gameObject.SetActive(true);
        source.clip = clip;
        source.volume = _audioSettings.SfxVolume;
        source.Play();

        _activeSfxSources.Add(source);
        StartCoroutine(ReturnToPoolAfterPlay(source, clip.length));
    }

    private IEnumerator ReturnToPoolAfterPlay(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        _activeSfxSources.Remove(source);
        _sfxPool.Enqueue(source);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (_musicSource.clip != clip)
        {
            _musicSource.clip = clip;
        }
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    private void ApplyMusicVolume()
    {
        _musicSource.volume = _audioSettings.MusicVolume;
    }

    private void OnAudioSettingsChanged()
    {
        ApplyMusicVolume();
        foreach (var source in _activeSfxSources)
        {
            source.volume = _audioSettings.SfxVolume;
        }
    }
}
