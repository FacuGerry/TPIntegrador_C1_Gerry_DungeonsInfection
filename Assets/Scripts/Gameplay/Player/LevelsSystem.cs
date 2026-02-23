using System;
using System.Collections;
using UnityEngine;

public class LevelsSystem : MonoBehaviour
{
    public static event Action<int> OnLevelUp;

    [SerializeField] private LevelsSO _data;

    private IEnumerator _corroutineLevelUp;

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
        float rand = UnityEngine.Random.value + 1;

        _data.levelBeforeBattle = _data.level;

        _data.experience += (int)(_data.experienceToGainAfterBattle * rand);
        if (_data.experience >= _data.experienceToLevelUp)
        {
            while (_data.experience >= _data.experienceToLevelUp)
            {
                _data.level++;
                _data.experience -= _data.experienceToLevelUp;
                yield return null;
            }
        }
        OnLevelUp?.Invoke(_data.level);
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
