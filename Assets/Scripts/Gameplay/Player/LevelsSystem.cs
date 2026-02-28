using System;
using System.Collections;
using UnityEngine;

public class LevelsSystem : MonoBehaviour
{
	public static event Action<int, int> OnLevelUp;

	[SerializeField] private LevelsSO _data;

	private IEnumerator _coroutineLevelUp;

	private void OnEnable()
	{
		CombatManager.OnPlayerWin += OnPlayerWin_GainXP;
	}

	private void OnDisable()
	{
		CombatManager.OnPlayerWin -= OnPlayerWin_GainXP;
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	private IEnumerator LevelingUp()
	{
		_data.levelBeforeBattle = _data.level;

		_data.experience += _data.experienceToGainAfterBattle;
		if (_data.experience >= _data.experienceToLevelUp)
		{
			while (_data.experience >= _data.experienceToLevelUp)
			{
				_data.level++;
				_data.experience -= _data.experienceToLevelUp;
				yield return null;
			}

			if (_data.level >= _data.maxLevel)
				_data.level = _data.maxLevel;

			OnLevelUp?.Invoke(_data.level, _data.levelBeforeBattle);
		}
		yield return null;
	}

	public void OnPlayerWin_GainXP()
	{
		if (_coroutineLevelUp != null)
			StopCoroutine(_coroutineLevelUp);

		_coroutineLevelUp = LevelingUp();
		StartCoroutine(_coroutineLevelUp);
	}
}
