using System;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public static event Action<BattleDefinitionSO> OnSwitchToFight;

    [SerializeField] private BattleDefinitionSO _battleToLoad;
    [SerializeField] private PositionInOverworldSO _playerPositionSo;
    [SerializeField] private Vector3 _playerPositionToSave;

    private void Start()
    {
        gameObject.SetActive(false);

        if (!_battleToLoad.isWon)
            gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerPositionSo.position = _playerPositionToSave;
        OnSwitchToFight?.Invoke(_battleToLoad);
        gameObject.SetActive(false);
    }

}
