using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _cameraPos;

    private void Start()
    {
        _cameraPos = transform.position;
    }

    private void Update()
    {
        _cameraPos.x = _target.position.x;
        _cameraPos.y = _target.position.y;
        transform.position = _cameraPos;
    }
}
