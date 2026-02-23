using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "GameData/Levels")]
public class LevelsSO : ScriptableObject
{
    [Header("Do not modify")]
    public int levelBeforeBattle;

    [Header("Level System")]
    public int level;
    public int maxLevel;
    public int experience;
    public int experienceToGainAfterBattle;
    public int experienceToLevelUp;
}
