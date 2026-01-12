using UnityEngine;

public class RoydMovement : MonoBehaviour
{
    [SerializeField] private KeyBindingsSO _keyBindings;

    public float speed;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        _rb.velocity = Vector2.zero;
        if (Input.GetKey(_keyBindings.up) || Input.GetKey(_keyBindings.up2))
        {
            _rb.AddForce(Vector2.up * (speed * Time.deltaTime), ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(_keyBindings.left) || Input.GetKey(_keyBindings.left2))
        {
            _rb.AddForce(Vector2.left * (speed * Time.deltaTime), ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKey(_keyBindings.down) || Input.GetKey(_keyBindings.down2))
        {
            _rb.AddForce(Vector2.down * (speed * Time.deltaTime), ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetKey(_keyBindings.right) || Input.GetKey(_keyBindings.right2))
        {
            _rb.AddForce(Vector2.right * (speed * Time.deltaTime), ForceMode2D.Force);
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }
}