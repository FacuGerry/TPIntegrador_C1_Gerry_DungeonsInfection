using System;
using UnityEngine;

public class LevelsSystem : MonoBehaviour
{
    public static event Action<int> OnLevelUp;

    [SerializeField] private UnlockingSpellsSO _data;

    private void OnEnable()
    {
        CombatManager.OnPlayerWin += OnPlayerWin_GainXP;
    }

    private void OnDisable()
    {
        CombatManager.OnPlayerWin -= OnPlayerWin_GainXP;
    }

    public void OnPlayerWin_GainXP()
    {
        float rand = UnityEngine.Random.value + 1;

        _data.experience += (int)(_data.experienceToGain * rand);
        if (_data.experience >= _data.experienceToLevelUp)
        {
            _data.level++;
            _data.experience = 0;

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
    }
}
