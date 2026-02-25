using UnityEngine;

[CreateAssetMenu(fileName = "UnlockingSpells", menuName = "GameData/UnlockingSpells")]
public class UnlockingSpellsSO : ScriptableObject
{
    [Header("Spells Unlocking")]
    public bool hasFireball;
    public bool hasMagicShield;
    public bool hasAbsorb;
    public bool hasHeal;
}
