using System.Collections;
using UnityEngine;

public class CombatAnimations : MonoBehaviour
{
    private IEnumerator _coroutinePlayerAttack;
    private IEnumerator _coroutineEnemyAttack;

    private void OnEnable()
    {
        CombatAction.OnPlayerAttackedEnemy += OnPlayerAttackorSpell_Animate;

        CombatAction.OnPlayerUsedSpellAnimate += OnPlayerAttackorSpell_Animate;

        EnemyTurnManager.OnAnimateEnemyAttack += OnEnemyAttack_Animate;
    }

    private void OnDisable()
    {
        CombatAction.OnPlayerAttackedEnemy -= OnPlayerAttackorSpell_Animate;

        CombatAction.OnPlayerUsedSpellAnimate -= OnPlayerAttackorSpell_Animate;

        EnemyTurnManager.OnAnimateEnemyAttack -= OnEnemyAttack_Animate;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator PlayerAttacking(Character player, Character enemy)
    {
        player.Animate(Character.CharacterStates.Attack);
        yield return new WaitForSeconds(0.7f);
        player.Animate(Character.CharacterStates.Idle);
        if (enemy.life <= 0)
        {
            enemy.Animate(Character.CharacterStates.Dead);
            yield return new WaitForSeconds(1f);
            enemy.gameObject.SetActive(false);
        }
        else
        {
            enemy.Animate(Character.CharacterStates.Hurt);
            yield return new WaitForSeconds(0.25f);
            enemy.Animate(Character.CharacterStates.Idle);
        }
    }

    private IEnumerator EnemyAttacking(Character enemy, Character player)
    {
        enemy.Animate(Character.CharacterStates.Attack);
        yield return new WaitForSeconds(0.7f);
        enemy.Animate(Character.CharacterStates.Idle);
        if (player.life <= 0)
        {
            player.Animate(Character.CharacterStates.Dead);
            yield return new WaitForSeconds(1f);
            player.gameObject.SetActive(false);
        }
        else
        {
            player.Animate(Character.CharacterStates.Hurt);
            yield return new WaitForSeconds(0.25f);
            player.Animate(Character.CharacterStates.Idle);
        }
    }

    public void OnPlayerAttackorSpell_Animate(Character player, Character enemy)
    {
        if (_coroutinePlayerAttack != null)
            StopCoroutine(_coroutinePlayerAttack);

        _coroutinePlayerAttack = PlayerAttacking(player, enemy);
        StartCoroutine(_coroutinePlayerAttack);
    }

    public void OnEnemyAttack_Animate(Character enemy, Character player)
    {
        if (_coroutineEnemyAttack != null)
            StopCoroutine(_coroutineEnemyAttack);

        _coroutineEnemyAttack = EnemyAttacking(enemy, player);
        StartCoroutine(_coroutineEnemyAttack);
    }
}
