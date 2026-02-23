using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckAllWinsAndRestart : MonoBehaviour
{
    public static event Action OnWinGame;

    [SerializeField] private string _sceneToLoad;
    [SerializeField] private UnlockingSpellsSO _spellsData;
    [SerializeField] private List<BattleDefinitionSO> _battles = new List<BattleDefinitionSO>();

    private IEnumerator _corroutineWaitWinScreen;

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
            if (_corroutineWaitWinScreen != null)
                StopCoroutine(_corroutineWaitWinScreen);

            _corroutineWaitWinScreen = WaitAndLoadWinScreen();
            StartCoroutine(_corroutineWaitWinScreen);
        }
    }

    public void OnMainMenuSelected_RestartGame()
    {
        foreach (BattleDefinitionSO battle in _battles)
        {
            battle.isWon = false;
        }

        _spellsData.hasFireball = false;
        _spellsData.hasIceWall = false;
        _spellsData.hasDarkShield = false;
        _spellsData.hasHealingRoot = false;
    }
}
