using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiSettingsMainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasMainMenu;
    [SerializeField] private Button _backBtn;
    [SerializeField] private float _fadeDuration;

    private CanvasGroup _canvasSettings;

    private IEnumerator _fadingCoroutine;
    private IEnumerator _fadingBackCoroutine;

    private void Awake()
    {
        _canvasSettings = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _backBtn.onClick.AddListener(BackClicked);

        _canvasSettings.alpha = 0;
        CanvasSettingsAppear(false);
    }

    private void OnEnable()
    {
        UiMainMenu.OnSettingsClicked += OnSettingsClicked_SettingsAppear;
    }

    private void OnDisable()
    {
        UiMainMenu.OnSettingsClicked -= OnSettingsClicked_SettingsAppear;
    }

    private void OnDestroy()
    {
        _backBtn.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }

    private IEnumerator FadingForSettings()
    {
        CanvasMainMenuAppear(false);
        float clock = _fadeDuration;
        while (clock > 0)
        {
            clock -= Time.deltaTime;
            float lerp = clock / _fadeDuration;
            _canvasMainMenu.alpha = lerp;
            yield return null;
        }
        clock = 0;
        while (clock < _fadeDuration)
        {
            clock += Time.deltaTime;
            float lerp = clock / _fadeDuration;
            _canvasSettings.alpha = lerp;
            yield return null;
        }
        CanvasSettingsAppear(true);
        yield return null;
    }

    private IEnumerator FadingForMainMenu()
    {
        CanvasSettingsAppear(false);
        float clock = _fadeDuration;
        while (clock > 0)
        {
            clock -= Time.deltaTime;
            float lerp = clock / _fadeDuration;
            _canvasSettings.alpha = lerp;
            yield return null;
        }
        clock = 0;
        while (clock < _fadeDuration)
        {
            clock += Time.deltaTime;
            float lerp = clock / _fadeDuration;
            _canvasMainMenu.alpha = lerp;
            yield return null;
        }
        CanvasMainMenuAppear(true);
        yield return null;
    }


    public void CanvasMainMenuAppear(bool isOn)
    {
        _canvasMainMenu.interactable = isOn;
        _canvasMainMenu.blocksRaycasts = isOn;
    }

    public void CanvasSettingsAppear(bool isOn)
    {
        _canvasSettings.interactable = isOn;
        _canvasSettings.blocksRaycasts = isOn;
    }

    public void BackClicked()
    {
        if (_fadingBackCoroutine != null)
            StopCoroutine(_fadingBackCoroutine);

        _fadingBackCoroutine = FadingForMainMenu();
        StartCoroutine(_fadingBackCoroutine);
    }

    public void OnSettingsClicked_SettingsAppear()
    {
        if (_fadingCoroutine != null)
            StopCoroutine(_fadingCoroutine);

        _fadingCoroutine = FadingForSettings();
        StartCoroutine(_fadingCoroutine);
    }
}
