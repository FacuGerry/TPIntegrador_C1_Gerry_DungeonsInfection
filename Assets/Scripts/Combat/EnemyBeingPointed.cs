using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBeingPointed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private Color _colorOnPointed;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _pointer.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointer.gameObject.SetActive(true);
        _spriteRenderer.color = _colorOnPointed;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteRenderer.color = Color.white;
        _pointer.gameObject.SetActive(false);
    }
}
