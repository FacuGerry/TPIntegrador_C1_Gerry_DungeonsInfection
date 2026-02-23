using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiWinAndLoseScreen : MonoBehaviour
{
    public static event Action OnMainMenuSelected;

    [SerializeField] private BattleDefinitionSO _finalBossBattle;
    [SerializeField] private CanvasGroup _canvasWin;
    [SerializeField] private CanvasGroup _canvasLose;
    [SerializeField] private Button _btnWin;
    [SerializeField] private Button _btnLose;
    [SerializeField] private string _sceneToLoadWin;
    [SerializeField] private string _sceneToLoadLose;

    private void Start()
    {
        CanvasWinAppear(false);
        CanvasLoseAppear(false);

        if (_finalBossBattle.isWon)
            CanvasWinAppear(true);
        else
            CanvasLoseAppear(true);

        _btnWin.onClick.AddListener(GoToMainMenu);
        _btnLose.onClick.AddListener(GoToOverworld);
    }

    private void OnDestroy()
    {
        _btnWin.onClick.RemoveAllListeners();
        _btnLose.onClick.RemoveAllListeners();
    }

    public void CanvasWinAppear(bool isOn)
    {
        _canvasWin.alpha = isOn ? 1f : 0f;
        _canvasWin.interactable = isOn;
        _canvasWin.blocksRaycasts = isOn;
    }

    public void CanvasLoseAppear(bool isOn)
    {
        _canvasLose.alpha = isOn ? 1f : 0f;
        _canvasLose.interactable = isOn;
        _canvasLose.blocksRaycasts = isOn;
    }

    public void GoToMainMenu()
    {
        OnMainMenuSelected?.Invoke();
        SceneManager.LoadScene(_sceneToLoadWin);
    }

    public void GoToOverworld()
    {
        SceneManager.LoadScene(_sceneToLoadLose);
    }
}
