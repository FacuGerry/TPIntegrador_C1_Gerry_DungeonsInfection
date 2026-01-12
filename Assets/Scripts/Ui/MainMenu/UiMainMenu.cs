using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMainMenu : MonoBehaviour
{
    public static event Action OnSettingsClicked;
    public static event Action OnCreditsClicked;

    [SerializeField] private string _sceneToLoad;

    [Header("Buttons Main Menu")]
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _creditsBtn;
    [SerializeField] private Button _exitBtn;

    private void Start()
    {
        _startBtn.onClick.AddListener(StartClicked);
        _settingsBtn.onClick.AddListener(SettingsClicked);
        _creditsBtn.onClick.AddListener(CreditsClicked);
        _exitBtn.onClick.AddListener(ExitClicked);
    }

    private void OnDestroy()
    {
        _startBtn.onClick.RemoveAllListeners();
        _settingsBtn.onClick.RemoveAllListeners();
        _creditsBtn.onClick.RemoveAllListeners();
        _exitBtn.onClick.RemoveAllListeners();
    }

    public void StartClicked()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void SettingsClicked()
    {
        OnSettingsClicked?.Invoke();
    }

    public void CreditsClicked()
    {
        OnCreditsClicked?.Invoke();
    }

    public void ExitClicked()
    {
        Application.Quit();
    }
}
