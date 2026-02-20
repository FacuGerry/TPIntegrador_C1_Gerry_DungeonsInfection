using System;
using System.Collections;
using UnityEngine;

public class LevelsSystem : MonoBehaviour
{
    public static event Action<int> OnLevelUp;

    [SerializeField] private UnlockingSpellsSO _data;

    private IEnumerator _corroutineLevelUp;

    private void OnEnable()
    {
        CombatManager.OnPlayerWin += OnPlayerWin_GainXP;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerWin -= OnPlayerWin_GainXP;
    }

    private IEnumerator LevelingUp()
    {
        float rand = UnityEngine.Random.value + 1;

        _data.experience += (int)(_data.experienceToGain * rand);
        if (_data.experience >= _data.experienceToLevelUp)
        {
            while (_data.experience >= _data.experienceToLevelUp)
            {
                _data.level++;
                _data.experience -= _data.experienceToLevelUp;
                yield return null;
            }

            if (_data.level >= _data.levelToUnlockFireball && !_data.hasFireball)
            {
                _data.hasFireball = true;
            }

            if (_data.level >= _data.levelToUnlockIceWall && !_data.hasIceWall)
            {
                _data.hasIceWall = true;
            }

            if (_data.level >= _data.levelToUnlockDarkShield && !_data.hasDarkShield)
            {
                _data.hasDarkShield = true;
            }

            if (_data.level >= _data.levelToUnlockHealingRoot && !_data.hasHealingRoot)
            {
                _data.hasHealingRoot = true;
            }

            OnLevelUp?.Invoke(_data.level);
        }

        yield return null;
    }

    public void OnPlayerWin_GainXP()
    {
        if (_corroutineLevelUp != null)
            StopCoroutine(_corroutineLevelUp);

        _corroutineLevelUp = LevelingUp();
        StartCoroutine(_corroutineLevelUp);
    }
}
