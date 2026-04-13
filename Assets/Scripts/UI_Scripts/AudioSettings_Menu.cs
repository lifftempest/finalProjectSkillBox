using UnityEngine;
using UnityEngine.UI;

public class AudioSettings_Menu : MonoBehaviour
{
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        SetUpSettings();

        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    private void SetUpSettings()
    {
        _musicSlider.value = _audioSettings.MusicVolume;
        _sfxSlider.value = _audioSettings.SfxVolume;
    }

    private void SetMusicVolume(float volume)
    {
        _audioSettings.MusicVolume = volume;
    }

    private void SetSfxVolume(float volume)
    {
        _audioSettings.SfxVolume = volume;
    }
}
