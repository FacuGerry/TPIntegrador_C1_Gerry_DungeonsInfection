using System;
using UnityEngine;

public class CheckForLevelUp : MonoBehaviour
{
    public static event Action<int> OnLevelUpChecked;

    [SerializeField] private LevelsSO _dataLevels;

    private void Start()
    {
        if (_dataLevels.levelBeforeBattle != _dataLevels.level)
        {
            _dataLevels.levelBeforeBattle = _dataLevels.level;
            OnLevelUpChecked?.Invoke(_dataLevels.level);
        }
    }
}
