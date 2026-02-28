using System.Collections.Generic;
using UnityEngine;

public class StatSetter : MonoBehaviour
{
    [SerializeField] private LoadingSO _dataLoad;
    [SerializeField] private CharacterDataSO _royd;
    [SerializeField] private CharacterDataSO _thane;
    [SerializeField] private UnlockingSpellsSO _spellsData;
    [SerializeField] private LevelsSO _levelsData;
    [SerializeField] private List<BattleDefinitionSO> _battles = new List<BattleDefinitionSO>();

    private void Start()
    {
        if (!_dataLoad.hasLoaded)
        {
            _spellsData.hasFireball = false;
            _spellsData.hasMagicShield = false;
            _spellsData.hasAbsorb = false;
            _spellsData.hasHeal = false;

            _levelsData.levelBeforeBattle = 1;
            _levelsData.level = 1;
            _levelsData.experience = 0;

            foreach (BattleDefinitionSO battle in _battles)
            {
                battle.isWon = false;
            }

            _royd.life = _royd.baseLife;
            _royd.attack = _royd.baseAttack;

            _royd.heal = _royd.baseHeal;
            _royd.defense = _royd.baseDefense;

            _thane.life = _thane.baseLife;
            _thane.attack = _thane.baseAttack;

            _thane.heal = _thane.baseHeal;
            _thane.defense = _thane.baseDefense;

            _dataLoad.hasLoaded = true;
        }
    }
}
