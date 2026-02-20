using UnityEngine;
using UnityEngine.UI;

public class MainMenuParallax : MonoBehaviour
{
    [SerializeField] private float _intensity;
    [SerializeField] private float _smoothSpeed;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Vector3 targetPosition = startPosition + new Vector3(mouseX, mouseY) * _intensity;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, _smoothSpeed * Time.deltaTime);
    }
}