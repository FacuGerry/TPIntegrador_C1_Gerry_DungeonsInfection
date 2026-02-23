using UnityEngine;

[CreateAssetMenu(fileName = "UnlockingSpells", menuName = "GameData/UnlockingSpells")]
public class UnlockingSpellsSO : ScriptableObject
{
    [Header("Spells Unlocking")]
    public bool hasFireball;
    public bool hasIceWall;
    public bool hasDarkShield;
    public bool hasHealingRoot;
}
