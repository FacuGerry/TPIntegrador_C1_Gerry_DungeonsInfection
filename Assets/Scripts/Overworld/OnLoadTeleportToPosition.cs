using UnityEngine;

public class OnLoadTeleportToPosition : MonoBehaviour
{
    [SerializeField] private PositionInOverworldSO _playerPosition;

    private void Start()
    {
        transform.position = _playerPosition.position;
    }
}
