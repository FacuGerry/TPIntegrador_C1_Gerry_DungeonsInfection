using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UiPlayersHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textRoyd;
    [SerializeField] private TextMeshProUGUI _textThane;

    private void OnEnable()
    {
        Character.OnDataSet += UpdateLife;
        Character.OnInfected += UpdateLife;
        CombatAction.OnPlayerUsedSpell += OnHeal_UpdateLife;
        EnemyTurnManager.OnEnemyAttack += UpdateLife;
    }

    private void OnDisable()
    {
        Character.OnDataSet -= UpdateLife;
        Character.OnInfected -= UpdateLife;
        CombatAction.OnPlayerUsedSpell -= OnHeal_UpdateLife;
        EnemyTurnManager.OnEnemyAttack -= UpdateLife;
    }

    public void UpdateLife(Character character)
    {
        if (character.data.name == "Royd")
        {
            _textRoyd.text = "HP " + character.life;
        }
        else if (character.data.name == "Thane")
        {
            _textThane.text = "HP " + character.life;
        }
    }

    public void OnHeal_UpdateLife(Character character, CombatAction.Spells spell)
    {
        if (spell == CombatAction.Spells.Heal)
        {
            if (character.data.name == "Royd")
            {
                _textRoyd.text = "HP " + character.life;
            }
            else if (character.data.name == "Thane")
            {
                _textThane.text = "HP " + character.life;
            }
        }
    }
}
