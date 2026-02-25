using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CombatManager : MonoBehaviour
{
    public static event Action OnBattleStart;

    public static event Action<Character, BattleDefinitionSO> OnSettingLifeToLevel;

    public static event Action OnWaitingForNewTurn;
    public static event Action OnWaitingForWin;

    public static event Action OnPlayerTurn;

    public static event Action OnPlayerWin;
    public static event Action OnPlayerLose;

    public static event Action OnPlayerEscaped;
    public static event Action OnPlayerNotEscaped;

    public static event Action<Character> OnIndicationsChange;

    public static event Action OnRoundEnd;

    private BattleDefinitionSO _battle;
    private CombatStates _state;

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;

    [Header("SpawnPoints")]
    [SerializeField] private Transform[] _spawnPointsPlayer = new Transform[2];
    [SerializeField] private List<Transform> _spawnPointsEnemies = new List<Transform>();

    private List<Character> _turnsList = new List<Character>();
    private List<Character> _playersList = new List<Character>();
    private List<Character> _enemiesList = new List<Character>();

    private int _turnCount;

    private CombatAction _combatAction;

    private Character _currentCharacter;

    private EnemyTurnManager _enemyTurnManager;

    private IEnumerator _waitingToStartCoroutine;
    private IEnumerator _waitingCoroutine;
    private IEnumerator _waitingWinCoroutine;

    private void Awake()
    {
        _combatAction = GetComponent<CombatAction>();
        _enemyTurnManager = GetComponent<EnemyTurnManager>();
    }

    private void Start()
    {
        _battle = GlobalManager.instance.currentBattle;
        if (_battle == null)
        {
            Debug.LogError("THERE WAS AN ERROR IN THE COMBAT'S LOAD");
            return;
        }
        Debug.Log("Combate cargado correctamente", gameObject);

        _enemyTurnManager.SetData(_battle);

        ChangeState(CombatStates.Start);
    }

    private void OnEnable()
    {
        CombatAction.OnPlayerEndTurn += OnEndTurn_ChangeTurnCount;

        EnemyTurnManager.OnEnemyAttackEnd += OnEndTurn_ChangeTurnCount;
        EnemyTurnManager.OnEnemyKilledPlayer += OnEnemyKilledPlayer_ChangeStateToLost;

        UiCombatButtons.OnAttackClicked += OnAttackClicked_ChangeState;
        UiCombatButtons.OnDefendClicked += OnDefendClicked_ChangeState;
        UiCombatButtons.OnEscapeClicked += OnEscapeClicked_RollDice;

        UiCombatButtons.OnFireballClicked += OnFireballClicked_ChangeState;
        UiCombatButtons.OnMagicShieldClicked += OnMagicShieldClicked_ChangeState;
        UiCombatButtons.OnAbsorbClicked += OnAbsorbClicked_ChangeState;
        UiCombatButtons.OnHealClicked += OnHealClicked_ChangeState;

        CombatAction.OnPlayerKillEnemy += OnPlayerKillEnemy_RemoveEnemy;
    }

    private void OnDisable()
    {
        CombatAction.OnPlayerEndTurn -= OnEndTurn_ChangeTurnCount;

        EnemyTurnManager.OnEnemyAttackEnd -= OnEndTurn_ChangeTurnCount;
        EnemyTurnManager.OnEnemyKilledPlayer -= OnEnemyKilledPlayer_ChangeStateToLost;

        UiCombatButtons.OnAttackClicked -= OnAttackClicked_ChangeState;
        UiCombatButtons.OnDefendClicked -= OnDefendClicked_ChangeState;
        UiCombatButtons.OnEscapeClicked -= OnEscapeClicked_RollDice;

        UiCombatButtons.OnFireballClicked -= OnFireballClicked_ChangeState;
        UiCombatButtons.OnMagicShieldClicked -= OnMagicShieldClicked_ChangeState;
        UiCombatButtons.OnAbsorbClicked -= OnAbsorbClicked_ChangeState;
        UiCombatButtons.OnHealClicked -= OnHealClicked_ChangeState;

        CombatAction.OnPlayerKillEnemy -= OnPlayerKillEnemy_RemoveEnemy;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitingToStart()
    {
        yield return new WaitForSeconds(1f);

        _turnCount = 0;
        StartTurn();
    }

    private IEnumerator WaitingForTurnChange(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeState(CombatStates.ChangeTurn);
    }

    private IEnumerator WaitingForWin()
    {
        yield return new WaitForSeconds(1.5f);
        ChangeState(CombatStates.Won);
    }

    public void ChangeState(CombatStates newState)
    {
        CombatStates prevState = _state;
        _state = newState;

        switch (_state)
        {
            case CombatStates.None:
                Debug.LogError("THERE IS NO COMBAT STATE");
                break;

            case CombatStates.Start:
                SpawnPlayers();
                SpawnEnemies();

                OnBattleStart?.Invoke();

                if (_waitingToStartCoroutine != null)
                    StopCoroutine(_waitingToStartCoroutine);

                _waitingToStartCoroutine = WaitingToStart();
                StartCoroutine(_waitingToStartCoroutine);
                break;

            case CombatStates.ChangeTurn:
                _turnCount++;
                if (_turnCount > (_turnsList.Count - 1))
                {
                    _turnCount = 0;
                    OnRoundEnd?.Invoke();
                }
                StartTurn();
                break;

            case CombatStates.EnemyTurn:
                StartEnemyTurn(_currentCharacter);
                break;

            case CombatStates.PlayerSelectAction:
                OnIndicationsChange?.Invoke(_currentCharacter);
                OnPlayerTurn?.Invoke();
                break;

            case CombatStates.PlayerSelectAttack:
                _combatAction.PlayerSelectEnemy(_currentCharacter);
                break;

            case CombatStates.PlayerSelectDefend:
                _combatAction.AddDefense(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.PlayerSelectFireball:
                _combatAction.UseFireball(_currentCharacter);
                break;

            case CombatStates.PlayerSelectMagicShield:
                _combatAction.UseMagicShield(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.PlayerSelectAbsorb:
                _combatAction.UseAbsorb(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.PlayerSelectHeal:
                _combatAction.UseHeal(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.Won:
                _battle.isWon = true;
                _state = CombatStates.None;
                OnPlayerWin?.Invoke();
                return;

            case CombatStates.Lost:
                foreach (Character player in _playersList)
                {
                    if (player.life <= 0)
                    {
                        OnIndicationsChange?.Invoke(player);
                        OnPlayerLose?.Invoke();
                        _battle.isWon = false;
                        break;
                    }
                }
                break;

            case CombatStates.Escaped:
                OnPlayerEscaped?.Invoke();
                _battle.isWon = false;
                break;
        }
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < _battle.players.Count; i++)
        {
            CharacterDataSO data = _battle.players[i];

            GameObject go = Instantiate(_playerPrefab, _spawnPointsPlayer[i].position, Quaternion.identity);

            Character character = go.GetComponent<Character>();
            character.SetData(data);

            _turnsList.Add(character);
            _playersList.Add(character);
        }
    }

    public void SpawnEnemies()
    {
        Transform positionEnemy = _spawnPointsEnemies[1];
        bool hasRemoved = false;
        for (int i = 0; i < _battle.enemies.Count; i++)
        {
            if (_battle.enemies.Count == 2 && !hasRemoved)
            {
                _spawnPointsEnemies.RemoveAt(0);
                hasRemoved = true;
            }

            CharacterDataSO data = _battle.enemies[i];

            GameObject go = Instantiate(_enemyPrefab, _spawnPointsEnemies[i].position, Quaternion.identity);

            Character character = go.GetComponent<Character>();
            character.SetData(data);

            _turnsList.Add(character);
            _enemiesList.Add(character);

            OnSettingLifeToLevel?.Invoke(character, _battle);
        }

        if (_battle.enemies.Count == 2)
            _spawnPointsEnemies.Insert(0, positionEnemy);
    }

    public void StartTurn()
    {
        _currentCharacter = _turnsList[_turnCount];

        if (_currentCharacter.data.isPlayer)
        {
            ChangeState(CombatStates.PlayerSelectAction);
        }
        else
        {
            ChangeState(CombatStates.EnemyTurn);
        }
    }

    public void WaitForSecondsAndChangeTurn(float seconds)
    {
        if (_waitingCoroutine != null)
            StopCoroutine(_waitingCoroutine);

        _waitingCoroutine = WaitingForTurnChange(seconds);
        StartCoroutine(_waitingCoroutine);
    }

    public void StartEnemyTurn(Character enemy)
    {
        enemy.InCharacterTurn_Move(_playersList);
    }

    public void OnEndTurn_ChangeTurnCount()
    {
        OnWaitingForNewTurn?.Invoke();
        WaitForSecondsAndChangeTurn(1f);
    }

    public void OnAttackClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectAttack);
    }

    public void OnDefendClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectDefend);
    }

    public void OnEscapeClicked_RollDice()
    {
        float rand = UnityEngine.Random.value;
        if (rand > _currentCharacter.data.valueToEscape)
        {
            ChangeState(CombatStates.Escaped);
        }
        else
        {
            OnPlayerNotEscaped?.Invoke();
            WaitForSecondsAndChangeTurn(2f);
        }
    }

    public void OnFireballClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectFireball);
    }

    public void OnMagicShieldClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectMagicShield);
    }

    public void OnAbsorbClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectAbsorb);
    }

    public void OnHealClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectHeal);
    }

    public void OnPlayerKillEnemy_RemoveEnemy(Character enemy)
    {
        _turnsList.Remove(enemy);
        _enemiesList.Remove(enemy);
        if (_enemiesList.Count == 0)
        {
            OnWaitingForWin?.Invoke();
            if (_waitingWinCoroutine != null)
                StopCoroutine(_waitingWinCoroutine);

            _waitingWinCoroutine = WaitingForWin();
            StartCoroutine(_waitingWinCoroutine);
        }
    }

    public void OnEnemyKilledPlayer_ChangeStateToLost()
    {
        ChangeState(CombatStates.Lost);
    }
}