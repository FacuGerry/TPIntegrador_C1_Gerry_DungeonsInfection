using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleDefinition", menuName = "BattleDefinition/BattleDefinition")]
public class BattleDefinitionSO : ScriptableObject
{
    public List<CharacterDataSO> players;
    public List<CharacterDataSO> enemies;

    public int battleLevel;
    public bool isWon;
}
