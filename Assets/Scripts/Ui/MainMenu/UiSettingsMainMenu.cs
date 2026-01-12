using UnityEngine;
using UnityEngine.UI;

public class UiSettingsMainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasMainMenu;
    [SerializeField] private Button _backBtn;
    private CanvasGroup _canvasSettings;

    private void Awake()
    {
        _canvasSettings = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _backBtn.onClick.AddListener(BackClicked);
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
    }

    public void CanvasMainMenuAppear(bool isOn)
    {
        _canvasMainMenu.alpha = isOn ? 1f : 0f;
        _canvasMainMenu.interactable = isOn;
        _canvasMainMenu.blocksRaycasts = isOn;
    }

    public void CanvasSettingsAppear(bool isOn)
    {
        _canvasSettings.alpha = isOn ? 1f : 0f;
        _canvasSettings.interactable = isOn;
        _canvasSettings.blocksRaycasts = isOn;
    }

    public void BackClicked()
    {
        CanvasMainMenuAppear(true);
        CanvasSettingsAppear(false);
    }

    public void OnSettingsClicked_SettingsAppear()
    {
        CanvasMainMenuAppear(false);
        CanvasSettingsAppear(true);
    }
}
