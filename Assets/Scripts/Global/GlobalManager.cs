using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    [NonSerialized] public BattleDefinitionSO currentBattle;
    [SerializeField] private string _sceneToLoadAfterLoading;
    [SerializeField] private string sceneToLoadForCombats;
    [SerializeField] private PositionInOverworldSO _playerPosition;
    [SerializeField] private Vector3 _playerInitialPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerPosition.position = _playerInitialPosition;

        SceneManager.LoadScene(_sceneToLoadAfterLoading);
    }

    private void OnEnable()
    {
        EnemyTrigger.OnSwitchToFight += OnEnemyTriggered_SwitchToFightScene;
    }

    private void OnDisable()
    {
        EnemyTrigger.OnSwitchToFight -= OnEnemyTriggered_SwitchToFightScene;
    }

    public void OnEnemyTriggered_SwitchToFightScene(BattleDefinitionSO battleDefinitionSO)
    {
        currentBattle = battleDefinitionSO;
        SceneManager.LoadScene(sceneToLoadForCombats);
    }
}