using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/Character")]
public class CharacterDataSO : ScriptableObject
{
    public string characterName;
    public int baseLife;
    public int life;
    public int baseAttack;
    public int attack;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isPlayer;

    [Header("IF NOT PLAYER, EVERYTHING BELOW IS = 0")]
    public int baseFireball;
    public int fireball;
    public int baseMagicShield;
    public int magicShield;
    public int baseAbsorb;
    public int absorb;
    public int baseHeal;
    public int heal;
    public int defense;
    public float valueToEscape;
    public int valueToAddWhenLevelUp;
    public int valueToAddWhenLevelUpDefense;
    public float chanceToCastSpell;
    public int lifeToQuitOnInfected;
}
