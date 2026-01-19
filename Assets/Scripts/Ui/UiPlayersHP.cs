using TMPro;
using UnityEngine;

public class UiPlayersHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textRoyd;
    [SerializeField] private TextMeshProUGUI _textThane;

    private void OnEnable()
    {
        Character.OnDataSet += UpdateLife;
        EnemyTurnManager.OnEnemyAttack += UpdateLife;
    }

    private void OnDisable()
    {
        Character.OnDataSet -= UpdateLife;
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
}
