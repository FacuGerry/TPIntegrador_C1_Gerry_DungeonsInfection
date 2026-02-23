using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class UiTextOverworld : MonoBehaviour
{
    public static event Action<bool> OnPauseForUnlocking;

    private CanvasGroup _canvas;
    [SerializeField] private KeyBindingsSO _keyBindings;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _textSpells;

    private IEnumerator _corroutine;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        CheckForLevelUp.OnLevelUpChecked += OnLevelUp_ShowText;
        SpellsUnlocking.OnBookObtained += OnBookObtained_ShowText;
    }

    private void OnDisable()
    {
        CheckForLevelUp.OnLevelUpChecked -= OnLevelUp_ShowText;
        SpellsUnlocking.OnBookObtained -= OnBookObtained_ShowText;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitingForInput()
    {
        Time.timeScale = 0f;
        OnPauseForUnlocking?.Invoke(true);
        bool isWaiting = true;
        while (isWaiting)
        {
            if (Input.GetKey(_keyBindings.interact))
            {
                isWaiting = false;
                CanvasAppear(false);
                Time.timeScale = 1f;
            }
            yield return null;
        }
        OnPauseForUnlocking?.Invoke(false);
    }

    public void OnLevelUp_ShowText(int level)
    {
        _textLevel.text += level.ToString();
        _textLevel.gameObject.SetActive(true);
        CanvasAppear(true);

        if (_corroutine != null)
            StopCoroutine(_corroutine);

        _corroutine = WaitingForInput();
        StartCoroutine(_corroutine);
    }

    public void OnBookObtained_ShowText(CombatAction.Spells spell)
    {
        switch (spell)
        {
            case CombatAction.Spells.Fireball:
                _textSpells.text += "Fireball\nUse it to attack your opponent";
                break;
            case CombatAction.Spells.IceWall:
                _textSpells.text += "Ice Wall\nUse it to defend yourself even more";
                break;
            case CombatAction.Spells.DarkShield:
                _textSpells.text += "Dark Shield\nUse it to defend AND heal yourself (if damaged)";
                break;
            case CombatAction.Spells.HealingRoot:
                _textSpells.text += "Healing Root­\nUse it to heal yourself";
                break;
        }

        _textSpells.gameObject.SetActive(true);
        CanvasAppear(true);

        if (_corroutine != null)
            StopCoroutine(_corroutine);

        _corroutine = WaitingForInput();
        StartCoroutine(_corroutine);
    }

    public void CanvasAppear(bool isOn)
    {
        _canvas.alpha = isOn ? 1f : 0f;
        _canvas.interactable = isOn;
        _canvas.blocksRaycasts = isOn;

        if (!isOn)
        {
            _textLevel.gameObject.SetActive(false);
            _textSpells.gameObject.SetActive(false);
        }
    }
}
