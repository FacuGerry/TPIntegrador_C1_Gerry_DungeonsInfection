using UnityEngine;

public class StairsCollision : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _locationToGo;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        _player.transform.position = _locationToGo.position;
    }
}
