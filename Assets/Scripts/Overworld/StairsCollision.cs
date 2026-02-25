using System;
using UnityEngine;

public class StairsCollision : MonoBehaviour
{
    public static event Action OnStairsUsed;

    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _locationToGo;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _player.transform.position = _locationToGo.position;
        OnStairsUsed?.Invoke();
    }
}
