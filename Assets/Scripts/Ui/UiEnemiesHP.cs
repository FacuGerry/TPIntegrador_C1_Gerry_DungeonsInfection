using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiEnemiesHP : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _textListNames = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _textListHP = new List<TextMeshProUGUI>();

    [SerializeField] private List<Transform> _textNamePositions = new List<Transform>();
    [SerializeField] private List<Transform> _textHPPositions = new List<Transform>();

    [SerializeField] private GameObject _textTitle;
    [SerializeField] private GameObject _textHP;

    private BattleDefinitionSO _battle;

    private void OnEnable()
    {
        CombatAction.OnUpdateEnemyLife += OnUpdateEnemyLife_UpdateLife;
    }

    private void OnDisable()
    {
        CombatAction.OnUpdateEnemyLife -= OnUpdateEnemyLife_UpdateLife;
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < _battle.enemies.Count; i++)
        {
            CharacterDataSO data = _battle.enemies[i];

            //GameObject go = Instantiate(_enemyPrefab, _textNamePositions[i].position, Quaternion.identity);
        }
    }

    public void OnUpdateEnemyLife_UpdateLife(Character enemy)
    {

    }
}
