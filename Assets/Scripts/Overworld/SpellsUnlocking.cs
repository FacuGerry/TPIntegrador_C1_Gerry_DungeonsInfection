using System;
using UnityEngine;

public class SpellsUnlocking : MonoBehaviour
{
    public static event Action<CombatAction.Spells> OnBookObtained;

    [SerializeField] private UnlockingSpellsSO _dataSpells;
    [SerializeField] private CombatAction.Spells _spells;
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (_spells)
        {
            case CombatAction.Spells.Fireball:
                _dataSpells.hasFireball = true;
                break;

            case CombatAction.Spells.IceWall:
                _dataSpells.hasIceWall = true;
                break;

            case CombatAction.Spells.DarkShield:
                _dataSpells.hasDarkShield = true;
                break;

            case CombatAction.Spells.HealingRoot:
                _dataSpells.hasHealingRoot = true;
                break;
        }

        OnBookObtained?.Invoke(_spells);
        gameObject.SetActive(false);
    }
}
