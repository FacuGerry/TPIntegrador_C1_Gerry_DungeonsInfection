using UnityEngine;

[CreateAssetMenu(fileName = "UnlockingSpells", menuName = "GameData/UnlockingSpells")]
public class UnlockingSpellsSO : ScriptableObject
{
    [Header("Spells Unlocking")]
    public bool hasFireball;
    public bool hasIceWall;
    public bool hasDarkShield;
    public bool hasHealingRoot;

    [Header("Level System")]
    public int level;
    public int experience;
    public int experienceToGain;
    public int experienceToLevelUp;

    [Header("Level to unlock spells")]
    public int levelToUnlockFireball;
    public int levelToUnlockIceWall;
    public int levelToUnlockDarkShield;
    public int levelToUnlockHealingRoot;
}
