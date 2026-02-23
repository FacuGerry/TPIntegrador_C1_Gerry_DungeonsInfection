using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButtonHoverSFXEvent : MonoBehaviour, IPointerEnterHandler
{
    public static event Action OnButtonHover;
    public static event Action OnButtonClick;
    private UnityEngine.UI.Button _btn;

    private void Awake()
    {
        _btn = GetComponent<UnityEngine.UI.Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(ButtonClicked);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButtonHover?.Invoke();
    }

    public void ButtonClicked()
    {
        OnButtonClick?.Invoke();
    }
}
