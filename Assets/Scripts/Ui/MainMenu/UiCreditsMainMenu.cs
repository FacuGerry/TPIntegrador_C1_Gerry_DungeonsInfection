using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiCreditsMainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasMainMenu;
    [SerializeField] private Button _backBtn;
    [SerializeField] private float _fadeDuration;
    private CanvasGroup _canvasCredits;

    private IEnumerator _fadingCoroutine;
    private IEnumerator _fadingBackCoroutine;

    private void Awake()
    {
        _canvasCredits = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _backBtn.onClick.AddListener(BackClicked);
        _canvasCredits.alpha = 0f;
        CanvasCreditsAppear(false);
    }

    private void OnEnable()
    {
        UiMainMenu.OnCreditsClicked += OnCreditsClicked_CreditsAppear;
    }

    private void OnDisable()
    {
        UiMainMenu.OnCreditsClicked -= OnCreditsClicked_CreditsAppear;
    }

    private void OnDestroy()
    {
        _backBtn.onClick.RemoveAllListeners();
        StopAllCoroutines();
    }

    private IEnumerator FadingForCredits()
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
            _canvasCredits.alpha = lerp;
            yield return null;
        }
        CanvasCreditsAppear(true);
        yield return null;
    }

    private IEnumerator FadingForMainMenu()
    {
        CanvasCreditsAppear(false);
        float clock = _fadeDuration;
        while (clock > 0)
        {
            clock -= Time.deltaTime;
            float lerp = clock / _fadeDuration;
            _canvasCredits.alpha = lerp;
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

    public void CanvasCreditsAppear(bool isOn)
    {
        _canvasCredits.interactable = isOn;
        _canvasCredits.blocksRaycasts = isOn;
    }

    public void BackClicked()
    {
        if (_fadingBackCoroutine != null)
            StopCoroutine(_fadingBackCoroutine);

        _fadingBackCoroutine = FadingForMainMenu();
        StartCoroutine(_fadingBackCoroutine);
    }

    public void OnCreditsClicked_CreditsAppear()
    {
        if (_fadingCoroutine != null)
            StopCoroutine(_fadingCoroutine);

        _fadingCoroutine = FadingForCredits();
        StartCoroutine(_fadingCoroutine);
    }

}
