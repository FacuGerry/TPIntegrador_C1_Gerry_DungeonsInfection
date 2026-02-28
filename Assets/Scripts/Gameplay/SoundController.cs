using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    [Header("Sources")]
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _ui;

    [Header("Pickables")]
    [SerializeField] private AudioClip _bookColected;

    [Header("Player in Overworld")]
    [SerializeField] private AudioClip _playerStepsSound;
    [SerializeField] private AudioClip _playerEntersStairs;

    [Header("Player in Combat")]
    [SerializeField] private AudioClip _playerAttackSound;
    [SerializeField] private AudioClip _playerHurtSound;
    [SerializeField] private AudioClip _playerDefendSound;
    [SerializeField] private AudioClip _playerDeathSound;

    [Header("Spells")]
    [SerializeField] private AudioClip _fireballSound;
    [SerializeField] private AudioClip _magicShieldSound;
    [SerializeField] private AudioClip _absorbSound;
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private AudioClip _noSpellsToUse;

    [Header("Infected")]
    [SerializeField] private AudioClip _infectedAttackSound;
    [SerializeField] private AudioClip _infectedHurtSound;
    [SerializeField] private AudioClip _infectedDeadSound;

    [Header("Virus")]
    [SerializeField] private AudioClip _virusAttackSound;
    [SerializeField] private AudioClip _virusHurtSound;

    [Header("Ui Sounds")]
    [SerializeField] private AudioClip _btnHover;
    [SerializeField] private AudioClip _btnClick;

    private IEnumerator _coroutineWalking;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        RoydMovement.OnPlayerMoves += OnPlayerMove_StartCoroutine;
        RoydMovement.OnPlayerStopsMoving += OnPlayerStopsMoving_StopCoroutine;

        EnemyTrigger.OnSwitchToFight += OnEnemyTrigger_StopSteps;

        SpellsUnlocking.OnBookObtained += OnBookObtained_SoundBookObtained;

        StairsCollision.OnStairsUsed += OnStairsUsed_SoundStairs;

        CombatAction.OnPlayerAttackedEnemy += OnPlayerAttackEnemy_SoundAttack;
        CombatAction.OnPlayerUsedSpell += OnPlayerUsedSpell_SoundSpell;
        CombatAction.OnPlayerHealedWithAbsorb += OnPlayerHealedWithAbsorb_SoundHealAbsorb;
        CombatAction.OnPlayerKillEnemy += OnPlayerKillEnemy_SoundEnemyDeath;

        EnemyTurnManager.OnEnemyKilledPlayer += OnPlayerDieByEnemy_SoundPlayerDead;
        EnemyTurnManager.OnShowEnemyAttacked += OnEnemyAttack_SoundEnemyAttack;

        Character.OnPlayerDie += OnPlayerDie_SoundPlayerDead;

        UiCombatButtons.OnNotHavingSpells += OnNotHavingSpells_SoundNotSpells;

        UiButtonHoverSFXEvent.OnButtonHover += OnButtonHover_SoundButtonHover;
        UiButtonHoverSFXEvent.OnButtonClick += OnButtonClick_SoundButtonClick;
    }

    private void OnDisable()
    {
        RoydMovement.OnPlayerMoves -= OnPlayerMove_StartCoroutine;
        RoydMovement.OnPlayerStopsMoving -= OnPlayerStopsMoving_StopCoroutine;

        EnemyTrigger.OnSwitchToFight -= OnEnemyTrigger_StopSteps;

        SpellsUnlocking.OnBookObtained -= OnBookObtained_SoundBookObtained;

        StairsCollision.OnStairsUsed -= OnStairsUsed_SoundStairs;

        CombatAction.OnPlayerAttackedEnemy -= OnPlayerAttackEnemy_SoundAttack;
        CombatAction.OnPlayerUsedSpell -= OnPlayerUsedSpell_SoundSpell;
        CombatAction.OnPlayerHealedWithAbsorb -= OnPlayerHealedWithAbsorb_SoundHealAbsorb;
        CombatAction.OnPlayerKillEnemy -= OnPlayerKillEnemy_SoundEnemyDeath;

        EnemyTurnManager.OnEnemyKilledPlayer -= OnPlayerDieByEnemy_SoundPlayerDead;
        EnemyTurnManager.OnShowEnemyAttacked -= OnEnemyAttack_SoundEnemyAttack;

        Character.OnPlayerDie -= OnPlayerDie_SoundPlayerDead;

        UiCombatButtons.OnNotHavingSpells -= OnNotHavingSpells_SoundNotSpells;

        UiButtonHoverSFXEvent.OnButtonHover -= OnButtonHover_SoundButtonHover;
        UiButtonHoverSFXEvent.OnButtonClick -= OnButtonClick_SoundButtonClick;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitingAndSound(AudioClip clip, float seconds)
    {
        bool isPlaying = true;
        while (isPlaying)
        {
            _sfx.PlayOneShot(clip);
            yield return new WaitForSeconds(seconds);
            yield return null;
        }
    }

    public void OnPlayerMove_StartCoroutine()
    {
        if (_coroutineWalking != null)
            StopCoroutine(_coroutineWalking);

        _coroutineWalking = WaitingAndSound(_playerStepsSound, 0.4f);
        StartCoroutine(_coroutineWalking);
    }

    public void OnPlayerStopsMoving_StopCoroutine()
    {
        if (_coroutineWalking != null)
            StopCoroutine(_coroutineWalking);
    }

    public void OnEnemyTrigger_StopSteps(BattleDefinitionSO battle)
    {
        if (_coroutineWalking != null)
            StopCoroutine(_coroutineWalking);
    }

    public void OnBookObtained_SoundBookObtained(CombatAction.Spells spell)
    {
        _sfx.PlayOneShot(_bookColected);
    }

    public void OnStairsUsed_SoundStairs()
    {
        _sfx.PlayOneShot(_playerEntersStairs);
    }

    public void OnPlayerAttackEnemy_SoundAttack(Character player, Character enemy)
    {
        _sfx.PlayOneShot(_playerAttackSound);

        switch (enemy.name)
        {
            case "Infected":
                _sfx.PlayOneShot(_infectedHurtSound);
                break;

            case "Virus":
                _sfx.PlayOneShot(_virusHurtSound);
                break;
        }
    }

    public void OnPlayerUsedSpell_SoundSpell(Character player, CombatAction.Spells spell)
    {
        switch (spell)
        {
            case CombatAction.Spells.Defend:
                _sfx.PlayOneShot(_playerDefendSound);
                break;
            case CombatAction.Spells.Fireball:
                _sfx.PlayOneShot(_fireballSound);
                break;
            case CombatAction.Spells.MagicShield:
                _sfx.PlayOneShot(_magicShieldSound);
                break;
            case CombatAction.Spells.Absorb:
                _sfx.PlayOneShot(_absorbSound);
                break;
            case CombatAction.Spells.Heal:
                _sfx.PlayOneShot(_healSound);
                break;
        }
    }

    public void OnPlayerHealedWithAbsorb_SoundHealAbsorb(Character player)
    {
        _sfx.PlayOneShot(_healSound);
    }

    public void OnPlayerKillEnemy_SoundEnemyDeath(Character enemy)
    {
        _sfx.PlayOneShot(_infectedDeadSound);
    }

    public void OnPlayerDieByEnemy_SoundPlayerDead()
    {
        _sfx.PlayOneShot(_playerDeathSound);
    }

    public void OnPlayerDie_SoundPlayerDead(Character player)
    {
        _sfx.PlayOneShot(_playerDeathSound);
    }

    public void OnEnemyAttack_SoundEnemyAttack(CharacterDataSO enemyData, Character player, string action)
    {
        switch (enemyData.name)
        {
            case "Infected":
                _sfx.PlayOneShot(_infectedAttackSound);
                break;

            case "Virus":
                _sfx.PlayOneShot(_virusAttackSound);
                break;
        }
    }

    public void OnNotHavingSpells_SoundNotSpells()
    {
        _sfx.PlayOneShot(_noSpellsToUse);
    }

    public void OnButtonHover_SoundButtonHover()
    {
        _ui.PlayOneShot(_btnHover);
    }

    public void OnButtonClick_SoundButtonClick()
    {
        _ui.PlayOneShot(_btnClick);
    }
}
