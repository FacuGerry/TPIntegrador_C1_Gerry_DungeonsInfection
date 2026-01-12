using System;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public static event Action<BattleDefinitionSO> OnSwitchToFight;

    [SerializeField] private BattleDefinitionSO _battleToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnSwitchToFight?.Invoke(_battleToLoad);
        gameObject.SetActive(false);
    }

}
