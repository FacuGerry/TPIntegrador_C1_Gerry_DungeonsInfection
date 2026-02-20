using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static event Action OnBattleStart;
    public static event Action OnWaitingForNewTurn;

    public static event Action OnPlayerTurn;

    public static event Action OnPlayerWin;
    public static event Action OnPlayerLose;

    public static event Action OnPlayerEscaped;
    public static event Action OnPlayerNotEscaped;

    public static event Action<Character> OnIndicationsChange;

    public static event Action OnRoundEnd;

    public enum CombatStates
    {
        None,
        Start,
        ChangeTurn,
        EnemyTurn,
        PlayerSelectAction,
        PlayerSelectAttack,
        PlayerSelectDefend,
        PlayerSelectFireball,
        PlayerSelectIceWall,
        PlayerSelectDarkShield,
        PlayerSelectHealingRoot,
        PlayerSelectRun,
        Animate,
        Won,
        Lost,
        Escaped
    }

    private BattleDefinitionSO _battle;
    private CombatStates _state;

    [Header("Prefabs")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;

    [Header("SpawnPoints")]
    [SerializeField] private Transform[] _spawnPointsPlayer = new Transform[2];
    [SerializeField] private Transform[] _spawnPointsEnemies = new Transform[5];

    private List<Character> _turnsList = new List<Character>();
    private List<Character> _playersList = new List<Character>();
    private List<Character> _enemiesList = new List<Character>();

    private int _turnCount;

    private CombatAction _combatAction;

    private Character _currentCharacter;

    private EnemyTurnManager _enemyTurnManager;

    private IEnumerator _waitingToStartCoroutine;
    private IEnumerator _waitingCoroutine;

    private void Awake()
    {
        _combatAction = GetComponent<CombatAction>();
        _enemyTurnManager = GetComponent<EnemyTurnManager>();
    }

    private void Start()
    {
        _battle = GlobalManager.Instance.currentBattle;
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
        UiCombatButtons.OnIceWallClicked += OnIceWallClicked_ChangeState;
        UiCombatButtons.OnDarkShieldClicked += OnDarkShieldClicked_ChangeState;
        UiCombatButtons.OnHealingRootClicked += OnHealingRootClicked_ChangeState;

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
        UiCombatButtons.OnIceWallClicked -= OnIceWallClicked_ChangeState;
        UiCombatButtons.OnDarkShieldClicked -= OnDarkShieldClicked_ChangeState;
        UiCombatButtons.OnHealingRootClicked -= OnHealingRootClicked_ChangeState;

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

            case CombatStates.PlayerSelectIceWall:
                _combatAction.UseIceWall(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.PlayerSelectDarkShield:
                _combatAction.UseDarkShield(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.PlayerSelectHealingRoot:
                _combatAction.UseHealingRoot(_currentCharacter);
                WaitForSecondsAndChangeTurn(1f);
                break;

            case CombatStates.Animate:
                break;

            case CombatStates.Won:
                OnPlayerWin?.Invoke();
                _battle.isWon = true;
                break;

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
        for (int i = 0; i < _battle.enemies.Count; i++)
        {
            CharacterDataSO data = _battle.enemies[i];

            GameObject go = Instantiate(_enemyPrefab, _spawnPointsEnemies[i].position, Quaternion.identity);

            Character character = go.GetComponent<Character>();
            character.SetData(data);

            _turnsList.Add(character);
            _enemiesList.Add(character);
        }
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

    public void OnIceWallClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectIceWall);
    }

    public void OnDarkShieldClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectDarkShield);
    }

    public void OnHealingRootClicked_ChangeState()
    {
        ChangeState(CombatStates.PlayerSelectHealingRoot);
    }

    public void OnPlayerKillEnemy_RemoveEnemy(Character enemy)
    {
        _turnsList.Remove(enemy);
        _enemiesList.Remove(enemy);
        if (_enemiesList.Count == 0)
        {
            ChangeState(CombatStates.Won);
        }
    }

    public void OnEnemyKilledPlayer_ChangeStateToLost()
    {
        ChangeState(CombatStates.Lost);
    }
}