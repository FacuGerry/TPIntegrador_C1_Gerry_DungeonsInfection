using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiEnemiesDefeatedOverworld : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<BattleDefinitionSO> _battles = new List<BattleDefinitionSO>();

    private void Start()
    {
        int wonBattles = 0;
        foreach (BattleDefinitionSO battle in _battles)
        {
            if (battle.isWon)
                wonBattles++;
        }
        _text.text = wonBattles.ToString() + " / " + _battles.Count;
    }
}
