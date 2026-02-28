using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/Character")]
public class CharacterDataSO : ScriptableObject
{
    public string characterName;
    public int life;
    public int attack;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isPlayer;

    [Header("IF NOT PLAYER, EVERYTHING BELOW IS = 0")]
    public float fireballMult;
    public float magicShieldMult;
    public float absorbMult;
    public int heal;
    public int defense;
    public float valueToEscape;
    public float chanceToCastSpell;
    public int lifeToQuitOnInfected;

    [Header("Base stats")]
    public int baseLife;
    public int baseAttack;
    public int baseHeal;
    public int baseDefense;

    [Header("Values to add on level up")]
    public int levelUpLife;
    public int levelUpAttack;
    public int levelUpDefense;
}
