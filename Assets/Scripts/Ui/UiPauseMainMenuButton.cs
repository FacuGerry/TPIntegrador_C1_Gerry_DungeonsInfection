using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiPauseMainMenuButton : MonoBehaviour
{
    public static event Action OnMainMenuClicked;

    [SerializeField] private Button _btn;
    [SerializeField] private string _mainMenuScene;

    private void Start()
    {
        _btn.onClick.AddListener(ButtonMainMenuClicked);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void ButtonMainMenuClicked()
    {
        OnMainMenuClicked?.Invoke();
        SceneManager.LoadScene(_mainMenuScene);
    }
}
