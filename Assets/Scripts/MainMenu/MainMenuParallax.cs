using UnityEngine;
using UnityEngine.UI;

public class MainMenuParallax : MonoBehaviour
{
    [SerializeField] private float _parallaxEfect;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        _image.rectTransform.position = Input.mousePosition / _parallaxEfect;
    }
}