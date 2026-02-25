using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCombatButtons : MonoBehaviour
{
    public static event Action OnAttackClicked;
    public static event Action OnDefendClicked;
    public static event Action OnSpellsClicked;
    public static event Action OnEscapeClicked;

    public static event Action OnFireballClicked;
    public static event Action OnMagicShieldClicked;
    public static event Action OnAbsorbClicked;
    public static event Action OnHealClicked;

    public static event Action OnNotHavingSpells;

    [Header("Action Buttons")]
    [SerializeField] private Button _attackBtn;
    [SerializeField] private Button _defendBtn;
    [SerializeField] private Button _spellsBtn;
    [SerializeField] private Button _escapeBtn;

    [Header("Spell Buttons")]
    [SerializeField] private Button _fireballBtn;
    [SerializeField] private Button _magicShieldBtn;
    [SerializeField] private Button _absorbBtn;
    [SerializeField] private Button _healBtn;

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup _canvasActions;
    [SerializeField] private CanvasGroup _canvasSpells;

    [Header("Spell Buttons Unlocking")]
    [SerializeField] private UnlockingSpellsSO _spellsUnlocked;

    private bool _isActionsPaused = false;
    private bool _isSpellsPaused = false;

    private void Start()
    {
        _attackBtn.onClick.AddListener(AttackClicked);
        _defendBtn.onClick.AddListener(DefendClicked);
        _spellsBtn.onClick.AddListener(SpellsClicked);
        _escapeBtn.onClick.AddListener(EscapeClicked);

        _fireballBtn.onClick.AddListener(FireballClicked);
        _magicShieldBtn.onClick.AddListener(MagicShieldClicked);
        _absorbBtn.onClick.AddListener(AbsorbClicked);
        _healBtn.onClick.AddListener(HealClicked);

        ToogleActionButtons(false);
        ToogleSpellsButtons(false);
    }

    private void OnEnable()
    {
        CombatManager.OnPlayerTurn += OnPlayerTurn_ShowActionButtons;

        PauseGame.OnPausingCombat += OnPause_ChangeButtons;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerTurn -= OnPlayerTurn_ShowActionButtons;

        PauseGame.OnPausingCombat -= OnPause_ChangeButtons;
    }

    private void OnDestroy()
    {
        _attackBtn.onClick.RemoveAllListeners();
        _defendBtn.onClick.RemoveAllListeners();
        _spellsBtn.onClick.RemoveAllListeners();
        _escapeBtn.onClick.RemoveAllListeners();

        _fireballBtn.onClick.RemoveAllListeners();
        _magicShieldBtn.onClick.RemoveAllListeners();
        _absorbBtn.onClick.RemoveAllListeners();
        _healBtn.onClick.RemoveAllListeners();
    }

    public void ToogleActionButtons(bool isOn)
    {
        _canvasActions.alpha = isOn ? 1f : 0f;
        _canvasActions.interactable = isOn;
        _canvasActions.blocksRaycasts = isOn;
    }

    public void OnPlayerTurn_ShowActionButtons()
    {
        ToogleActionButtons(true);
    }

    public void OnPause_ChangeButtons()
    {
        if (_canvasActions.alpha == 1f)
        {
            _isActionsPaused = true;
            ToogleActionButtons(false);
            return;
        }
        if (_canvasSpells.alpha == 1f) 
        {
            _isSpellsPaused = true;
            ToogleSpellsButtons(false);
            return;
        }

        if (_isActionsPaused)
        {
            _isActionsPaused = false;
            ToogleActionButtons(true);
        }
        if (_isSpellsPaused)
        {
            _isSpellsPaused = false;
            ToogleSpellsButtons(true);
        }
    }

    public void ToogleSpellsButtons(bool isOn)
    {
        if (!_spellsUnlocked.hasFireball && !_spellsUnlocked.hasMagicShield && !_spellsUnlocked.hasAbsorb && !_spellsUnlocked.hasHeal && isOn)
        {
            ToogleActionButtons(true);
            OnNotHavingSpells?.Invoke();
            return;
        }

        _fireballBtn.gameObject.SetActive(_spellsUnlocked.hasFireball);
        _magicShieldBtn.gameObject.SetActive(_spellsUnlocked.hasMagicShield);
        _absorbBtn.gameObject.SetActive(_spellsUnlocked.hasAbsorb);
        _healBtn.gameObject.SetActive(_spellsUnlocked.hasHeal);

        _canvasSpells.alpha = isOn ? 1f : 0f;
        _canvasSpells.interactable = isOn;
        _canvasSpells.blocksRaycasts = isOn;
    }

    public void AttackClicked()
    {
        ToogleActionButtons(false);
        OnAttackClicked?.Invoke();
    }

    public void DefendClicked()
    {
        ToogleActionButtons(false);
        OnDefendClicked?.Invoke();
    }

    public void SpellsClicked()
    {
        ToogleActionButtons(false);
        ToogleSpellsButtons(true);
        OnSpellsClicked?.Invoke();
    }

    public void EscapeClicked()
    {
        ToogleActionButtons(false);
        OnEscapeClicked?.Invoke();
    }

    public void FireballClicked()
    {
        ToogleSpellsButtons(false);
        OnFireballClicked?.Invoke();
    }

    public void MagicShieldClicked()
    {
        ToogleSpellsButtons(false);
        OnMagicShieldClicked?.Invoke();
    }

    public void AbsorbClicked()
    {
        ToogleSpellsButtons(false);
        OnAbsorbClicked?.Invoke();
    }

    public void HealClicked()
    {
        ToogleSpellsButtons(false);
        OnHealClicked?.Invoke();
    }
}
