using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private WinPanel _winPanelBeh;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private AudioClip _gameplayAudioClip;
    [SerializeField] private AudioClip _winJingleClip;
    [SerializeField] private AudioClip _loseJingleClip;

    private GameObject _currentWindow;
    private GameObject _previousWindow;

    private void Awake()
    {
        _currentWindow = _mainMenuUI;
        _currentWindow.SetActive(true);
        ShowCursor();

        EventManager.PlayerDeath += ExecuteDeathBeh;
        EventManager.PlayerWin += ExecuteWinBeh;
        EventManager.OnSettingsKeyPressed += OpenSettingsPanel;
    }

    private void OnDestroy()
    {
        EventManager.PlayerDeath -= ExecuteDeathBeh;
        EventManager.PlayerWin -= ExecuteWinBeh;
        EventManager.OnSettingsKeyPressed -= OpenSettingsPanel;
    }

    public void StartGame()
    {
        _playerController.enabled = true;
        SwitchWindow(_gameplayUI);
        AudioHandler.Instance.PlayMusic(_gameplayAudioClip);
        HideCursor();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SettingsButtonAction(bool isOpened)
    {
        if (isOpened)
        {
            ShowCursor();
            SwitchWindow(_settingsMenu);
            Time.timeScale = 0f;
            Debug.Log($"{_currentWindow}, {_previousWindow}");
        }
        else
        {
            if (_previousWindow == _gameplayUI)
            {
                HideCursor();
            }
            SwitchWindow(_previousWindow);
            Time.timeScale = 1f;
            Debug.Log($"{_currentWindow}, {_previousWindow}");
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OpenSettingsPanel()
    {
        if (_settingsMenu.activeInHierarchy == false)
        {
            SettingsButtonAction(true);
        }
        else
        {
            SettingsButtonAction(false);
            //HideCursor();
        }
    }

    private void ExecuteDeathBeh()
    {
        SwitchWindow(_losePanel);
        AudioHandler.Instance.StopMusic();
        AudioHandler.Instance.PlaySfx(_loseJingleClip);
        ShowCursor();
    }

    private void ExecuteWinBeh()
    {
        _playerController.enabled = false;
        SwitchWindow(_winPanel);
        _winPanelBeh.SetStats();
        AudioHandler.Instance.StopMusic();
        AudioHandler.Instance.PlaySfx(_winJingleClip);
        ShowCursor();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void SwitchWindow(GameObject newWindow)
    {
        _previousWindow = _currentWindow;
        _currentWindow.SetActive(false);
        _currentWindow = newWindow;
        _currentWindow.SetActive(true);
    }
}
