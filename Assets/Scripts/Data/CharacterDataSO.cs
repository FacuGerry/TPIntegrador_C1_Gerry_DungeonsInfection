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
    public int fireball;
    public int iceWall;
    public int darkShield;
    public int healingRoot;
    public int defense;
    public float valueToEscape;
    public int valueToAddWhenLevelUp;
    public int valueToAddWhenLevelUpDefense;
}
