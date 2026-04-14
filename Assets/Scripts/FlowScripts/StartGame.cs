using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private AudioClip _gameplayAudioClip;

    public void StartGameFlow()
    {
        _playerController.enabled = true;
        _mainMenuUI.SetActive(false);
        _gameplayUI.SetActive(true);
        AudioHandler.Instance.PlayMusic(_gameplayAudioClip);
    }
}
