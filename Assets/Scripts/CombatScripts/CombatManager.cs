using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private BattleDefinitionSO _battle;

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

    private void Start()
    {
        _battle = GlobalManager.Instance.currentBattle;
        if (_battle == null)
        {
            Debug.LogError("THERE WAS AN ERROR IN THE COMBAT'S LOAD");
            return;
        }
        Debug.Log("Combate cargado correctamente", gameObject);

        SpawnPlayers();
        SpawnEnemies();

        _turnCount = 0;
        ChangeTurns();
    }

    private void OnEnable()
    {
        EnemyTurnManager.OnEnemyAttacked += OnEndTurn_ChangeTurnCount;
    }

    private void OnDisable()
    {
        EnemyTurnManager.OnEnemyAttacked -= OnEndTurn_ChangeTurnCount;
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

    public void ChangeTurns()
    {
        Character currentCharacter = _turnsList[_turnCount];
        if (currentCharacter.life <= 0)
        {
            _turnsList.Remove(currentCharacter);
            OnEndTurn_ChangeTurnCount();
        }
        else
        {
            currentCharacter.InCharacterTurn_Move(_playersList, _enemiesList);
        }
    }

    public void OnEndTurn_ChangeTurnCount()
    {
        _turnCount++;
        if (_turnCount > (_turnsList.Count - 1))
            _turnCount = 0;
        ChangeTurns();
    }
}