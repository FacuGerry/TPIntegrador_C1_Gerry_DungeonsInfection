using System;
using System.Collections;
using UnityEngine;

public partial class CombatAction : MonoBehaviour
{
	public static event Action OnPlayerEndTurn;

	public static event Action OnPlayerSelectEnemy;
	public static event Action<Character, Character> OnPlayerAttackedEnemy;
	public static event Action<Character> OnPlayerKillEnemy;

	public static event Action<Character> OnUpdateEnemyLife;

	public static event Action<Character, Spells> OnPlayerUsedSpell;
	public static event Action<Character, Character> OnPlayerUsedSpellAnimate;
	public static event Action<Character> OnPlayerHealedWithAbsorb;

	public static event Action<Character> OnFailedToCast;

	private Spells _spells = Spells.None;

	private IEnumerator _coroutineSelectEnemy;
	private IEnumerator _coroutineFailedToCast;

	private bool _isFireball = false;

	private void OnEnable()
	{
		EnemyTurnManager.OnPlayerUsedAbsorb += OnPlayerUsedAbsorb_AddLife;
		CombatManager.OnWaitingForWin += OnWin_StopCoroutine;
	}

	private void OnDisable()
	{
		EnemyTurnManager.OnPlayerUsedAbsorb += OnPlayerUsedAbsorb_AddLife;
		CombatManager.OnWaitingForWin -= OnWin_StopCoroutine;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		_coroutineSelectEnemy = null;
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
						enemy.life -= (int)(player.data.attack * player.data.fireballMult);
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
					}

					isWaiting = false;
				}
			}
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		OnPlayerEndTurn?.Invoke();
	}

	private IEnumerator FailingToCast(Character player)
	{
		OnFailedToCast?.Invoke(player);
		yield return new WaitForSeconds(1f);
		OnPlayerEndTurn?.Invoke();
	}

	public void OnPlayerUsedAbsorb_AddLife(Character player)
	{
		player.life += (int)(player.data.defense * player.data.absorbMult);
		player.isAbsorbOn = false;

		if (player.life > player.maxLife)
			player.life = player.maxLife;

		OnPlayerHealedWithAbsorb?.Invoke(player);
	}

	public void OnWin_StopCoroutine()
	{
		StopAllCoroutines();
	}

	public bool TryToCast(Character player)
	{
		float rand = UnityEngine.Random.value;
		if (rand > player.data.chanceToCastSpell)
			return true;
		else
			return false;
	}

	public void PlayerSelectEnemy(Character player)
	{
		if (_coroutineSelectEnemy != null)
			StopCoroutine(_coroutineSelectEnemy);

		_coroutineSelectEnemy = WatingForInput(player);
		StartCoroutine(_coroutineSelectEnemy);
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
		if (TryToCast(player))
		{
			_isFireball = true;
			PlayerSelectEnemy(player);
			_spells = Spells.Fireball;
		}
		else
		{
			if (_coroutineFailedToCast != null)
				StopCoroutine(_coroutineFailedToCast);

			_coroutineFailedToCast = FailingToCast(player);
			StartCoroutine(_coroutineFailedToCast);
		}
	}

	public void UseMagicShield(Character player)
	{
		if (TryToCast(player))
		{
			player.defense += (int)(player.data.defense * player.data.magicShieldMult);
			player.isMagicShieldOn = true;
			_spells = Spells.MagicShield;
			OnPlayerUsedSpell?.Invoke(player, _spells);
			_spells = Spells.None;
		}
		else
		{
			if (_coroutineFailedToCast != null)
				StopCoroutine(_coroutineFailedToCast);

			_coroutineFailedToCast = FailingToCast(player);
			StartCoroutine(_coroutineFailedToCast);
		}
	}

	public void UseAbsorb(Character player)
	{
		if (TryToCast(player))
		{
			player.defense += (int)(player.data.defense * player.data.absorbMult);
			player.isAbsorbOn = true;
			_spells = Spells.Absorb;
			OnPlayerUsedSpell?.Invoke(player, _spells);
			_spells = Spells.None;
		}
		else
		{
			if (_coroutineFailedToCast != null)
				StopCoroutine(_coroutineFailedToCast);

			_coroutineFailedToCast = FailingToCast(player);
			StartCoroutine(_coroutineFailedToCast);
		}
	}

	public void UseHeal(Character player)
	{
		if (TryToCast(player))
		{
			player.life += player.data.heal;
			if (player.life > player.maxLife)
				player.life = player.maxLife;
			_spells = Spells.Heal;
			OnPlayerUsedSpell?.Invoke(player, _spells);
			_spells = Spells.None;
		}
		else
		{
			if (_coroutineFailedToCast != null)
				StopCoroutine(_coroutineFailedToCast);

			_coroutineFailedToCast = FailingToCast(player);
			StartCoroutine(_coroutineFailedToCast);
		}
	}
}
