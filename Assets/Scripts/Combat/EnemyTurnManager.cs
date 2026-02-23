using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static event Action<CharacterDataSO> OnEnemyTurnStart;
    public static event Action<Character> OnEnemyAttack;
    public static event Action<CharacterDataSO, Character, string> OnShowEnemyAttacked;
    public static event Action OnEnemyAttackEnd;
    public static event Action OnEnemyKilledPlayer;
    public static event Action<Character> OnPlayerUsedDarkShield;

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

        if (rand < 0.15f && enemy.life < (enemy.life / 1.5))
        {
            // heal
            _action = "healed";
            enemy.life += ((_battleDefinition.battleLevel / 10) + 1) * _infectedAttacks.heal;
            if (enemy.life > enemy.data.life)
                enemy.life = enemy.data.life;
        }
        else if ((rand < 0.15f && enemy.life > (enemy.life / 1.5)) || (rand >= 0.15f && rand < 0.45f))
        {
            // scratch
            _action = "scratched";
            _attack *= (_battleDefinition.battleLevel / 10) + 1;
        }
        else if (rand >= 0.45f && rand < 0.65f)
        {
            // bite
            _action = "bit";
            _attack = ((_battleDefinition.battleLevel / 10) + 1) * _infectedAttacks.bite;
        }
        else if (rand >= 0.65f)
        {
            // infect
            _action = "infected";
            _attack = ((_battleDefinition.battleLevel / 10) + 1) * _infectedAttacks.infect;
            _playerAttacked.isInfected = true;
        }

        if (_action != "healed")
        {
            int finalDmg = (_attack - _playerAttacked.defense);

            if (_playerAttacked.isDarkShieldOn)
                finalDmg -= _playerAttacked.data.darkShield;

            if (_playerAttacked.isIceWallOn)
                finalDmg -= _playerAttacked.data.iceWall;

            if (finalDmg <= 0)
            {
                _attack = 0;
                _action = "failed to attack";
            }
            else
                _attack = finalDmg;

            _playerAttacked.life -= _attack;

            if (_playerAttacked.life <= 0)
                _playerAttacked.life = 0;


            _attack = enemy.data.attack;
            OnEnemyAttack?.Invoke(_playerAttacked);
            if (_playerAttacked.isDarkShieldOn)
            {
                OnPlayerUsedDarkShield?.Invoke(_playerAttacked);
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
    }

    private IEnumerator VirusAttack(Character enemy)
    {


        int finalDmg = (_attack - _playerAttacked.defense);

        if (_playerAttacked.isDarkShieldOn)
            finalDmg -= _playerAttacked.data.darkShield;

        if (_playerAttacked.isIceWallOn)
            finalDmg -= _playerAttacked.data.iceWall;

        if (finalDmg <= 0)
        {
            _attack = 0;
            _action = "failed to attack";
        }
        else
            _attack = finalDmg;

        _playerAttacked.life -= _attack;

        if (_playerAttacked.life <= 0)
            _playerAttacked.life = 0;


        _attack = enemy.data.attack;
        OnEnemyAttack?.Invoke(_playerAttacked);
        if (_playerAttacked.isDarkShieldOn)
        {
            OnPlayerUsedDarkShield?.Invoke(_playerAttacked);
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
        OnEnemyTurnStart?.Invoke(enemy.data);

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
