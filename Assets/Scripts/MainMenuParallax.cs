using UnityEngine;

public class MainMenuParallax : MonoBehaviour
{
    [SerializeField] private float _parallaxEfect;
    private RectTransform _position;

    private void Awake()
    {
        _position = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _position.position = Input.mousePosition / _parallaxEfect;
    }
}