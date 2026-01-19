using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    public static event Action<Character> OnEnemyAttack;
    public static event Action OnEnemyAttackEnd;

    private Character playerAttacked;
    private int attack;

    private IEnumerator _courroutineInfectedAttack;

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

    private IEnumerator InfectedAttack(CharacterDataSO data)
    {
        float rand = UnityEngine.Random.value;

        if (rand < 0.15f && data.life < (data.life / 1.5))
        {
            // heal
            data.life += 3;
        }
        else if ((rand < 0.15f && data.life > (data.life / 1.5)) || (rand >= 0.15f && rand < 0.45f))
        {
            // scratch
            playerAttacked.life -= attack;
        }
        else if (rand >= 0.45f && rand < 0.65f)
        {
            // bite
            attack += 3;
            playerAttacked.life -= attack;
        }
        else if (rand >= 0.65f)
        {
            // infect
            attack = 1;
            playerAttacked.life -= attack;
        }
        attack = data.attack;
        OnEnemyAttack?.Invoke(playerAttacked);

        yield return new WaitForSeconds(1f);

        OnEnemyAttackEnd?.Invoke();
    }

    public void OnEnemyTurn_SelectPlayerToAttack(CharacterDataSO data, List<Character> players)
    {
        attack = data.attack;
        Character royd = players[0];
        Character thane = players[1];

        float playerToAttack = UnityEngine.Random.value;
        switch (playerToAttack)
        {
            case < .5f:
                playerAttacked = royd;
                break;
            case >= .5f:
                playerAttacked = thane;
                break;
        }

        switch (data.characterName)
        {
            case "Infected":
                if (_courroutineInfectedAttack != null)
                    StopCoroutine(_courroutineInfectedAttack);

                _courroutineInfectedAttack = InfectedAttack(data);
                StartCoroutine(_courroutineInfectedAttack);
                break;
            case "Virus":
                break;
        }
    }
}
