using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static event Action OnPlayerTurn;

    public enum CombatStates
    {
        None,
        Start,
        ChangeTurn,
        EnemyTurn,
        PlayerSelectAction,
        PlayerSelectSpell,
        PlayerSelectEnemy,
        Animate,
        Won,
        Lost
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

    private void Awake()
    {
        _combatAction = GetComponent<CombatAction>();
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

        ChangeState(CombatStates.Start);
    }

    private void OnEnable()
    {
        CombatAction.OnPlayerEndTurn += OnEndTurn_ChangeTurnCount;

        EnemyTurnManager.OnEnemyAttackEnd += OnEndTurn_ChangeTurnCount;

        UiCombatButtons.OnAttackClicked += OnPlayerAttack_ChooseEnemy;
    }

    private void OnDisable()
    {
        CombatAction.OnPlayerEndTurn -= OnEndTurn_ChangeTurnCount;

        EnemyTurnManager.OnEnemyAttackEnd -= OnEndTurn_ChangeTurnCount;

        UiCombatButtons.OnAttackClicked -= OnPlayerAttack_ChooseEnemy;
    }

    public void ChangeState(CombatStates newState)
    {
        _state = newState;

        switch (newState)
        {
            case CombatStates.None:
                Debug.LogError("THERE IS NO COMBAT STATE");
                break;

            case CombatStates.Start:
                SpawnPlayers();
                SpawnEnemies();

                _turnCount = 0;
                StartTurn();
                break;

            case CombatStates.ChangeTurn:
                _turnCount++;
                if (_turnCount > (_turnsList.Count - 1))
                    _turnCount = 0;
                StartTurn();
                break;

            case CombatStates.EnemyTurn:
                StartEnemyTurn(_currentCharacter);
                break;

            case CombatStates.PlayerSelectAction:
                OnPlayerTurn?.Invoke();
                break;

            case CombatStates.PlayerSelectSpell:
                break;

            case CombatStates.PlayerSelectEnemy:
                _combatAction.PlayerSelectEnemy(_currentCharacter.data);
                break;

            case CombatStates.Animate:
                break;

            case CombatStates.Won:
                break;

            case CombatStates.Lost:
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
        if (_currentCharacter.life <= 0)
        {
            _turnsList.Remove(_currentCharacter);
            if (!_currentCharacter.data.isPlayer)
                _enemiesList.Remove(_currentCharacter);
            OnEndTurn_ChangeTurnCount();
            return;
        }

        if (_currentCharacter.data.isPlayer)
        {
            ChangeState(CombatStates.PlayerSelectAction);
        }
        else
        {
            ChangeState(CombatStates.EnemyTurn);
        }
    }

    public void StartEnemyTurn(Character enemy)
    {
        enemy.InCharacterTurn_Move(_playersList);
    }

    public void OnEndTurn_ChangeTurnCount()
    {
        ChangeState(CombatStates.ChangeTurn);
    }

    public void OnPlayerAttack_ChooseEnemy()
    {
        ChangeState(CombatStates.PlayerSelectEnemy);
    }
}