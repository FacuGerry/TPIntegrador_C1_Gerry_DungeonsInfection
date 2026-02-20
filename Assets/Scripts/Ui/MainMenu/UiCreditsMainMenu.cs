using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiCreditsMainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasMainMenu;
    [SerializeField] private Button _backBtn;
    [SerializeField] private float _fadeDuration;
    private CanvasGroup _canvasCredits;

    private IEnumerator _fadingCorroutine;
    private IEnumerator _fadingBackCorroutine;

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
        if (_fadingBackCorroutine != null)
            StopCoroutine(_fadingBackCorroutine);

        _fadingBackCorroutine = FadingForMainMenu();
        StartCoroutine(_fadingBackCorroutine);
    }

    public void OnCreditsClicked_CreditsAppear()
    {
        if (_fadingCorroutine != null)
            StopCoroutine(_fadingCorroutine);

        _fadingCorroutine = FadingForCredits();
        StartCoroutine(_fadingCorroutine);
    }

}
