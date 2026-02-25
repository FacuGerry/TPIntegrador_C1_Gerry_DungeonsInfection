using System;
using UnityEngine;

public class SpellsUnlocking : MonoBehaviour
{
    public static event Action<CombatAction.Spells> OnBookObtained;

    [SerializeField] private UnlockingSpellsSO _dataSpells;
    [SerializeField] private CombatAction.Spells _spells;

    private void Start()
    {
        switch (_spells)
        {
            case CombatAction.Spells.Fireball:
                if (_dataSpells.hasFireball)
                    gameObject.SetActive(false);
                break;

            case CombatAction.Spells.MagicShield:
                if (_dataSpells.hasMagicShield)
                    gameObject.SetActive(false);
                break;

            case CombatAction.Spells.Absorb:
                if (_dataSpells.hasAbsorb)
                    gameObject.SetActive(false);
                break;

            case CombatAction.Spells.Heal:
               if (_dataSpells.hasHeal)
                    gameObject.SetActive(false);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (_spells)
        {
            case CombatAction.Spells.Fireball:
                _dataSpells.hasFireball = true;
                break;

            case CombatAction.Spells.MagicShield:
                _dataSpells.hasMagicShield = true;
                break;

            case CombatAction.Spells.Absorb:
                _dataSpells.hasAbsorb = true;
                break;

            case CombatAction.Spells.Heal:
                _dataSpells.hasHeal = true;
                break;
        }

        OnBookObtained?.Invoke(_spells);
        gameObject.SetActive(false);
    }
}
