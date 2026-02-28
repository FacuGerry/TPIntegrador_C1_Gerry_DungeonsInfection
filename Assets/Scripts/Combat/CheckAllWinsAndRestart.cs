using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckAllWinsAndRestart : MonoBehaviour
{
    public static event Action OnWinGame;
    public static event Action OnLoseGame;

    [SerializeField] private string _sceneToLoad;
    [SerializeField] private string _sceneWin;
    [SerializeField] private List<CharacterDataSO> _players = new List<CharacterDataSO>();
    [SerializeField] private UnlockingSpellsSO _spellsData;
    [SerializeField] private LevelsSO _levelsData;
    [SerializeField] private List<BattleDefinitionSO> _battles = new List<BattleDefinitionSO>();

    private IEnumerator _coroutineWaitWinScreen;

    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(_sceneWin))
        {
            OnPlayerWin_CheckWinGame();
        }
    }

    private void OnEnable()
    {
        CombatManager.OnPlayerWin += OnPlayerWin_CheckWinGame;

        UiWinAndLoseScreen.OnMainMenuSelected += OnMainMenuSelected_RestartGame;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerWin -= OnPlayerWin_CheckWinGame;

        UiWinAndLoseScreen.OnMainMenuSelected -= OnMainMenuSelected_RestartGame;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator WaitAndLoadWinScreen()
    {
        yield return new WaitForSeconds(2f);
        if (_sceneToLoad != ".")
            SceneManager.LoadScene(_sceneToLoad);
    }

    public void OnPlayerWin_CheckWinGame()
    {
        int winBattles = 0;
        foreach (BattleDefinitionSO battle in _battles)
        {
            if (battle.isWon)
                winBattles++;
        }

        if (winBattles == _battles.Count)
        {
            OnWinGame?.Invoke();
            if (_coroutineWaitWinScreen != null)
                StopCoroutine(_coroutineWaitWinScreen);

            _coroutineWaitWinScreen = WaitAndLoadWinScreen();
            StartCoroutine(_coroutineWaitWinScreen);
        }
        else
            OnLoseGame?.Invoke();
    }

    public void OnMainMenuSelected_RestartGame()
    {
        foreach (BattleDefinitionSO battle in _battles)
        {
            battle.isWon = false;
        }

        foreach (CharacterDataSO player in _players)
        {
            player.life = player.baseLife;
            player.attack = player.baseAttack;
            player.heal = player.baseHeal;
            player.defense = player.baseDefense;
        }

        _spellsData.hasFireball = false;
        _spellsData.hasMagicShield = false;
        _spellsData.hasAbsorb = false;
        _spellsData.hasHeal = false;

        _levelsData.levelBeforeBattle = 1;
        _levelsData.level = 1;
        _levelsData.experience = 0;
    }
}
