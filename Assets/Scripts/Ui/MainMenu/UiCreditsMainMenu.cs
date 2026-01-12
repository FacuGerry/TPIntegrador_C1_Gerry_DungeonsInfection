using UnityEngine;
using UnityEngine.UI;

public class UiCreditsMainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasMainMenu;
    [SerializeField] private Button _backBtn;
    private CanvasGroup _canvasCredits;

    private void Awake()
    {
        _canvasCredits = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _backBtn.onClick.AddListener(BackClicked);
    }

    private void OnEnable()
    {
        UiMainMenu.OnCreditsClicked += OnSettingsClicked_SettingsAppear;
    }

    private void OnDisable()
    {
        UiMainMenu.OnCreditsClicked -= OnSettingsClicked_SettingsAppear;
    }

    private void OnDestroy()
    {
        _backBtn.onClick.RemoveAllListeners();
    }

    public void CanvasMainMenuAppear(bool isOn)
    {
        _canvasMainMenu.alpha = isOn ? 1f : 0f;
        _canvasMainMenu.interactable = isOn;
        _canvasMainMenu.blocksRaycasts = isOn;
    }

    public void CanvasCreditsAppear(bool isOn)
    {
        _canvasCredits.alpha = isOn ? 1f : 0f;
        _canvasCredits.interactable = isOn;
        _canvasCredits.blocksRaycasts = isOn;
    }

    public void BackClicked()
    {
        CanvasMainMenuAppear(true);
        CanvasCreditsAppear(false);
    }

    public void OnSettingsClicked_SettingsAppear()
    {
        CanvasMainMenuAppear(false);
        CanvasCreditsAppear(true);
    }

}
