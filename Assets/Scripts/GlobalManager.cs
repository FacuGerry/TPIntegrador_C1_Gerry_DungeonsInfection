using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    [NonSerialized] public BattleDefinitionSO currentBattle;
    public string sceneToLoad;

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
        SceneManager.LoadScene("Overworld");
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
        SceneManager.LoadScene(sceneToLoad);
    }
}