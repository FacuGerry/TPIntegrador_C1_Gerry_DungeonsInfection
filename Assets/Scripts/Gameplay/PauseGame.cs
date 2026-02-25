using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static event Action<bool> OnPause;
    public static event Action OnPausingCombat;

    [SerializeField] private KeyBindingsSO _keyData;

    [SerializeField] private CanvasGroup _canvasPause;

    [SerializeField] private Button _btnBack;

    [SerializeField] private float _fadeDuration;

    private bool _isPaused;

    private bool _canBePaused;

    private IEnumerator _coroutineFading;

    private void Start()
    {
        _isPaused = false;
        _canBePaused = true;
        CanvasAppear(_isPaused);

        _btnBack.onClick.AddListener(OnBackClicked);
    }

    private void OnEnable()
    {
        UiTextOverworld.OnPauseForUnlocking += OnPauseForUnlocking_DisablePause;

        UiPauseMainMenuButton.OnMainMenuClicked += OnMainMenuClicked_DisablePause;
    }

    private void Update()
    {
        if (Input.GetKeyUp(_keyData.pause) && _canBePaused)
        {
            switch (_isPaused)
            {
                case true:
                    _isPaused = false;

                    if (_coroutineFading != null)
                        StopCoroutine(_coroutineFading);

                    _coroutineFading = FadingForPause(false);
                    StartCoroutine(_coroutineFading);
                    break;

                case false:
                    _isPaused = true;

                    if (_coroutineFading != null)
                        StopCoroutine(_coroutineFading);

                    _coroutineFading = FadingForPause(true);
                    StartCoroutine(_coroutineFading);
                    break;
            }
        }
    }

    private void OnDisable()
    {
        UiTextOverworld.OnPauseForUnlocking -= OnPauseForUnlocking_DisablePause;

        UiPauseMainMenuButton.OnMainMenuClicked -= OnMainMenuClicked_DisablePause;
    }

    private void OnDestroy()
    {
        _btnBack.onClick.RemoveAllListeners();

        StopAllCoroutines();
    }

    private IEnumerator FadingForPause(bool isOn)
    {
        OnPausingCombat?.Invoke();
        switch (isOn)
        {
            case true:
                float clockTrue = 0;
                while (clockTrue < _fadeDuration)
                {
                    clockTrue += Time.deltaTime;
                    float lerp = clockTrue / _fadeDuration;
                    _canvasPause.alpha = lerp;
                    yield return null;
                }
                _canvasPause.interactable = isOn;
                _canvasPause.blocksRaycasts = isOn;
                Time.timeScale = 0f;
                OnPause?.Invoke(_isPaused);
                break;

            case false:
                Time.timeScale = 1f;
                _canvasPause.interactable = isOn;
                _canvasPause.blocksRaycasts = isOn;
                float clockFalse = _fadeDuration;
                while (clockFalse > 0)
                {
                    clockFalse -= Time.deltaTime;
                    float lerp = clockFalse / _fadeDuration;
                    _canvasPause.alpha = lerp;
                    yield return null;
                }
                OnPause?.Invoke(_isPaused);
                break;
        }
        yield return null;
    }

    public void OnPauseForUnlocking_DisablePause(bool isPause)
    {
        _canBePaused = !isPause;
    }

    public void OnMainMenuClicked_DisablePause()
    {
        OnBackClicked();
    }

    public void CanvasAppear(bool isOn)
    {
        _canvasPause.alpha = isOn ? 1f : 0f;
        _canvasPause.interactable = isOn;
        _canvasPause.blocksRaycasts = isOn;
    }

    public void OnBackClicked()
    {
        _isPaused = false;

        if (_coroutineFading != null)
            StopCoroutine(_coroutineFading);

        _coroutineFading = FadingForPause(false);
        StartCoroutine(_coroutineFading);
    }
}
