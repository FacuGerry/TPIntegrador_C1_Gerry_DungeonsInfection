using System;
using System.Collections;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    public static event Action OnPlayerEndTurn;

    public static event Action OnPlayerSelectEnemy;
    public static event Action<Character, Character> OnPlayerAttackedEnemy;
    public static event Action<Character> OnPlayerKillEnemy;
    public static event Action<Character> OnPlayerKillEnemyShowText;

    public static event Action<Character> OnUpdateEnemyLife;

    public static event Action<Character, Spells> OnPlayerUsedSpell;
    public static event Action<Character, Character> OnPlayerUsedSpellAnimate;
    public static event Action<Character> OnPlayerHealedWithDarkShield;

    public enum Spells
    {
        None = 0,
        Defend,
        Fireball,
        IceWall,
        DarkShield,
        HealingRoot
    }

    private Spells _spells = Spells.None;

    private IEnumerator _corroutineSelectEnemy;

    private bool _isFireball = false;

    private void OnEnable()
    {
        EnemyTurnManager.OnPlayerUsedDarkShield += OnPlayerUsedDarkShield_AddLife;
    }

    private void OnDisable()
    {
        EnemyTurnManager.OnPlayerUsedDarkShield += OnPlayerUsedDarkShield_AddLife;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _corroutineSelectEnemy = null;
    }

    private IEnumerator WatingForInput(Character player)
    {
        OnPlayerSelectEnemy?.Invoke();
        bool isWaiting = true;
        while (isWaiting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 direction = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D ray = Physics2D.Raycast(mousePos, direction);

                if (ray.collider != null && ray.collider.TryGetComponent(out Character enemy))
                {
                    if (_isFireball)
                    {
                        enemy.life -= player.data.fireball;
                        OnPlayerUsedSpell?.Invoke(player, _spells);
                        _spells = Spells.None;
                        _isFireball = false;
                        OnPlayerUsedSpellAnimate?.Invoke(player, enemy);
                    }
                    else
                    {
                        enemy.life -= player.data.attack;
                        OnPlayerAttackedEnemy?.Invoke(player, enemy);
                    }

                    OnUpdateEnemyLife?.Invoke(enemy);

                    Debug.Log(enemy.name + " life is " + enemy.life, enemy.gameObject);

                    if (enemy.life <= 0)
                    {
                        OnPlayerKillEnemy?.Invoke(enemy);
                        yield return new WaitForSeconds(1f);
                        OnPlayerKillEnemyShowText?.Invoke(enemy);
                    }

                    isWaiting = false;
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        OnPlayerEndTurn?.Invoke();
    }

    public void PlayerSelectEnemy(Character player)
    {
        if (_corroutineSelectEnemy != null)
            StopCoroutine(_corroutineSelectEnemy);

        _corroutineSelectEnemy = WatingForInput(player);
        StartCoroutine(_corroutineSelectEnemy);
    }

    public void AddDefense(Character player)
    {
        player.defense += player.data.defense;
        _spells = Spells.Defend;
        OnPlayerUsedSpell?.Invoke(player, _spells);
        _spells = Spells.None;
    }

    public void UseFireball(Character player)
    {
        _isFireball = true;
        PlayerSelectEnemy(player);
        _spells = Spells.Fireball;
    }

    public void UseIceWall(Character player)
    {
        player.defense += player.data.iceWall;
        player.isIceWallOn = true;
        _spells = Spells.IceWall;
        OnPlayerUsedSpell?.Invoke(player, _spells);
        _spells = Spells.None;
    }

    public void UseDarkShield(Character player)
    {
        player.defense += player.data.darkShield;
        player.isDarkShieldOn = true;
        _spells = Spells.DarkShield;
        OnPlayerUsedSpell?.Invoke(player, _spells);
        _spells = Spells.None;
    }

    public void UseHealingRoot(Character player)
    {
        player.life += player.data.healingRoot;
        if (player.life > player.maxLife)
            player.life = player.maxLife;
        _spells = Spells.HealingRoot;
        OnPlayerUsedSpell?.Invoke(player, _spells);
        _spells = Spells.None;
    }

    public void OnPlayerUsedDarkShield_AddLife(Character player)
    {
        player.life += player.data.darkShield;
        player.isDarkShieldOn = false;
        if (player.life > player.maxLife)
            player.life = player.maxLife;

        OnPlayerHealedWithDarkShield?.Invoke(player);
    }
}
