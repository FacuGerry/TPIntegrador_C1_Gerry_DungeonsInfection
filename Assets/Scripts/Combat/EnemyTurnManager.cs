using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static event Action<Character> OnEnemyAttack;
    public static event Action<CharacterDataSO, Character, string> OnShowEnemyAttacked;
    public static event Action OnEnemyAttackEnd;
    public static event Action OnEnemyKilledPlayer;
    public static event Action<Character> OnPlayerUsedAbsorb;

    public static event Action<Character, Character> OnAnimateEnemyAttack;

    [SerializeField] private InfectedAttacksSO _infectedAttacks;

    private BattleDefinitionSO _battleDefinition;

    private Character _playerAttacked;
    private int _attack;
    private string _action;

    private IEnumerator _courroutineInfectedAttack;
    private IEnumerator _courroutineVirusAttack;

    private void OnEnable()
    {
        Character.OnEnemyTurn += OnEnemyTurn_SelectPlayerToAttack;
    }

    private void OnDisable()
    {
        Character.OnEnemyTurn -= OnEnemyTurn_SelectPlayerToAttack;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator InfectedAttack(Character enemy)
    {
        float rand = UnityEngine.Random.value;

        if (rand < 0.33f)
        {
            // scratch
            _action = "scratched";
            _attack = (int)(_battleDefinition.battleLevel * _infectedAttacks.scratch);
        }
        else if (rand >= 0.33f && rand < 0.66f)
        {
            // bite
            _action = "bit";
            _attack = (int)(_battleDefinition.battleLevel * _infectedAttacks.bite);
        }
        else if (rand >= 0.66f)
        {
            // infect
            _action = "infected";
            _attack = (int)(_battleDefinition.battleLevel * _infectedAttacks.infect);
        }

        int finalDmg = (_attack - _playerAttacked.defense);

        if (_playerAttacked.isAbsorbOn)
            finalDmg -= (int)(_playerAttacked.data.defense * _playerAttacked.data.absorbMult);

        if (_playerAttacked.isMagicShieldOn)
            finalDmg -= (int)(_playerAttacked.data.defense * _playerAttacked.data.magicShieldMult);

        if (finalDmg <= 0)
        {
            _attack = 0;
            _action = "failed to attack";
        }
        else
            _attack = finalDmg;

        if (finalDmg > 0 && _action == "infected")
            _playerAttacked.isInfected = true;

        _playerAttacked.life -= _attack;

        if (_playerAttacked.life <= 0)
            _playerAttacked.life = 0;


        _attack = enemy.data.attack;
        OnEnemyAttack?.Invoke(_playerAttacked);
        if (_playerAttacked.isAbsorbOn)
        {
            OnPlayerUsedAbsorb?.Invoke(_playerAttacked);
        }

        OnShowEnemyAttacked?.Invoke(enemy.data, _playerAttacked, _action);

        if (_action != "failed to attack")
            OnAnimateEnemyAttack?.Invoke(enemy, _playerAttacked);

        if (_playerAttacked.life <= 0)
        {
            OnEnemyKilledPlayer?.Invoke();
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        OnEnemyAttackEnd?.Invoke();
    }

    private IEnumerator VirusAttack(Character enemy)
    {
        float finalDmg = (_attack - _playerAttacked.defense) * _battleDefinition.battleLevel;

        if (_playerAttacked.isAbsorbOn)
            finalDmg -= (int)(_playerAttacked.data.defense * _playerAttacked.data.absorbMult);

        if (_playerAttacked.isMagicShieldOn)
            finalDmg -= (int)(_playerAttacked.data.defense * _playerAttacked.data.magicShieldMult);

        if (finalDmg < 0)
        {
            _attack = 0;
            _action = "failed to attack";
        }
        else
        {
            _attack = (int)finalDmg;
            _action = "attacked";
        }

        _playerAttacked.life -= _attack;

        if (_playerAttacked.life <= 0)
            _playerAttacked.life = 0;

        _attack = enemy.data.attack;
        OnEnemyAttack?.Invoke(_playerAttacked);
        if (_playerAttacked.isAbsorbOn)
        {
            OnPlayerUsedAbsorb?.Invoke(_playerAttacked);
        }

        OnShowEnemyAttacked?.Invoke(enemy.data, _playerAttacked, _action);

        if (_action != "failed to attack")
            OnAnimateEnemyAttack?.Invoke(enemy, _playerAttacked);

        if (_playerAttacked.life <= 0)
        {
            OnEnemyKilledPlayer?.Invoke();
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        OnEnemyAttackEnd?.Invoke();
    }

    public void SetData(BattleDefinitionSO battleData)
    {
        _battleDefinition = battleData;
    }

    public void OnEnemyTurn_SelectPlayerToAttack(Character enemy, List<Character> players)
    {
        _attack = enemy.data.attack;
        Character royd = players[0];
        Character thane = players[1];

        float playerToAttack = UnityEngine.Random.value;
        switch (playerToAttack)
        {
            case < .5f:
                _playerAttacked = royd;
                break;
            case >= .5f:
                _playerAttacked = thane;
                break;
        }

        switch (enemy.data.characterName)
        {
            case "Infected":
                if (_courroutineInfectedAttack != null)
                    StopCoroutine(_courroutineInfectedAttack);

                _courroutineInfectedAttack = InfectedAttack(enemy);
                StartCoroutine(_courroutineInfectedAttack);
                break;
            case "Virus":
                if (_courroutineVirusAttack != null)
                    StopCoroutine(_courroutineVirusAttack);

                _courroutineVirusAttack = VirusAttack(enemy);
                StartCoroutine(_courroutineVirusAttack);
                break;
        }
    }
}
