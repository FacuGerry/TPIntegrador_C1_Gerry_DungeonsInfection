using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/Character")]
public class CharacterDataSO : ScriptableObject
{
    public string characterName;
    public int life;
    public int attack;
    public SpriteRenderer sprite;
    public Animator animator;
    public bool isPlayer;
}
