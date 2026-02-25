using System;
using System.Net.Http.Headers;
using UnityEngine;

public class RoydMovement : MonoBehaviour
{
    public static event Action<int> OnAnimate;
    public static event Action OnPlayerMoves;
    public static event Action OnPlayerStopsMoving;

    public enum Actions
    {
        None = -1,
        IdleDown,
        IdleSide,
        IdleUp,
        MoveSide,
        MoveUp,
        MoveDown,
        Interact
    }

    [SerializeField] private KeyBindingsSO _keyBindings;

    private Rigidbody2D _rb;

    private Actions _currentAction = Actions.IdleDown;
    private Actions _previousAction = Actions.None;

    public float speed;

    private bool _isPause = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentAction = Actions.IdleDown;
        _isPause = false;
    }

    private void OnEnable()
    {
        PauseGame.OnPause += OnPause_PausePlayer;

        UiTextOverworld.OnPauseForUnlocking += OnPause_PausePlayer;
    }

    private void Update()
    {
        if (!_isPause)
        {
            FSM();
        }
    }

    private void OnDisable()
    {
        PauseGame.OnPause -= OnPause_PausePlayer;

        UiTextOverworld.OnPauseForUnlocking -= OnPause_PausePlayer;
    }

    public void OnPause_PausePlayer(bool isPause)
    {
        _isPause = isPause;
    }

    public void FSM()
    {
        switch (_currentAction)
        {
            case Actions.None:
                Debug.LogError("THERE IS NO ACTION LOADED");
                break;

            case Actions.IdleDown:
                // OnEnter
                if (_previousAction != _currentAction)
                {
                    _previousAction = _currentAction;
                    SetVelocity();
                    OnAnimate?.Invoke((int)_currentAction);
                    OnPlayerStopsMoving?.Invoke();
                }

                // Update
                if (Input.GetKey(_keyBindings.left) || Input.GetKey(_keyBindings.right))
                {
                    _currentAction = Actions.MoveSide;
                }
                else if (Input.GetKey(_keyBindings.up))
                {
                    _currentAction = Actions.MoveUp;
                }
                else if (Input.GetKey(_keyBindings.down))
                {
                    _currentAction = Actions.MoveDown;
                }
                break;

            case Actions.IdleSide:
                // OnEnter
                if (_previousAction != _currentAction)
                {
                    _previousAction = _currentAction;
                    SetVelocity();
                    OnAnimate?.Invoke((int)_currentAction);
                    OnPlayerStopsMoving?.Invoke();
                }

                // Update
                if (Input.GetKey(_keyBindings.left) || Input.GetKey(_keyBindings.right))
                {
                    _currentAction = Actions.MoveSide;
                }
                else if (Input.GetKey(_keyBindings.up))
                {
                    _currentAction = Actions.MoveUp;
                }
                else if (Input.GetKey(_keyBindings.down))
                {
                    _currentAction = Actions.MoveDown;
                }
                break;

            case Actions.IdleUp:
                // OnEnter
                if (_previousAction != _currentAction)
                {
                    _previousAction = _currentAction;
                    SetVelocity();
                    OnAnimate?.Invoke((int)_currentAction);
                    OnPlayerStopsMoving?.Invoke();
                }

                // Update
                if (Input.GetKey(_keyBindings.left) || Input.GetKey(_keyBindings.right))
                {
                    _currentAction = Actions.MoveSide;
                }
                else if (Input.GetKey(_keyBindings.up))
                {
                    _currentAction = Actions.MoveUp;
                }
                else if (Input.GetKey(_keyBindings.down))
                {
                    _currentAction = Actions.MoveDown;
                }
                break;

            case Actions.MoveSide:
                // OnEnter
                if (_previousAction != _currentAction)
                {
                    _previousAction = _currentAction;
                    OnAnimate?.Invoke((int)_currentAction);
                    OnPlayerMoves?.Invoke();
                }

                // Update
                MoveSide();

                if (!Input.GetKey(_keyBindings.left) && !Input.GetKey(_keyBindings.right))
                {
                    _currentAction = Actions.IdleSide;
                }
                else if (Input.GetKey(_keyBindings.up) && !Input.GetKey(_keyBindings.left) && !Input.GetKey(_keyBindings.right))
                {
                    _currentAction = Actions.MoveUp;
                }
                else if (Input.GetKey(_keyBindings.down) && !Input.GetKey(_keyBindings.left) && !Input.GetKey(_keyBindings.right))
                {
                    _currentAction = Actions.MoveDown;
                }
                break;

            case Actions.MoveUp:
                // OnEnter
                if (_previousAction != _currentAction)
                {
                    _previousAction = _currentAction;
                    OnAnimate?.Invoke((int)_currentAction);
                    OnPlayerMoves?.Invoke();
                }

                // Update
                MoveUp();

                if (!Input.GetKey(_keyBindings.up))
                {
                    _currentAction = Actions.IdleUp;
                }
                else if ((Input.GetKey(_keyBindings.left) || Input.GetKey(_keyBindings.right)) && !Input.GetKey(_keyBindings.up))
                {
                    _currentAction = Actions.MoveSide;
                }
                else if (Input.GetKey(_keyBindings.down) && !Input.GetKey(_keyBindings.up))
                {
                    _currentAction = Actions.MoveDown;
                }
                break;

            case Actions.MoveDown:
                // OnEnter
                if (_previousAction != _currentAction)
                {
                    _previousAction = _currentAction;
                    OnAnimate?.Invoke((int)_currentAction);
                    OnPlayerMoves?.Invoke();
                }

                // Update
                MoveDown();

                if (!Input.GetKey(_keyBindings.down))
                {
                    _currentAction = Actions.IdleDown;
                }
                else if ((Input.GetKey(_keyBindings.left) || Input.GetKey(_keyBindings.right)) && !Input.GetKey(_keyBindings.down))
                {
                    _currentAction = Actions.MoveSide;
                }
                else if (Input.GetKey(_keyBindings.up) && !Input.GetKey(_keyBindings.down))
                {
                    _currentAction = Actions.MoveUp;
                }
                break;
        }
    }

    public void SetVelocity()
    {
        _rb.velocity = Vector2.zero;
    }

    public void MoveSide()
    {
        _rb.velocity = Vector2.zero;

        if (Input.GetKey(_keyBindings.left) && !Input.GetKey(_keyBindings.right))
        {
            if (transform.rotation.y != 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            _rb.AddForce(Vector2.left * speed, ForceMode2D.Force);
        }

        if (Input.GetKey(_keyBindings.right) && !Input.GetKey(_keyBindings.left))
        {
            if (transform.rotation.y != 180)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            _rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
        }

        _rb.velocity = Vector2.zero;
    }

    public void MoveUp()
    {
        _rb.velocity = Vector2.zero;

        if (transform.rotation.y != 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        if (Input.GetKey(_keyBindings.up))
            _rb.AddForce(Vector2.up * speed, ForceMode2D.Force);

        _rb.velocity = Vector2.zero;
    }

    public void MoveDown()
    {
        _rb.velocity = Vector2.zero;

        if (transform.rotation.y != 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        if (Input.GetKey(_keyBindings.down))
            _rb.AddForce(Vector2.down * speed, ForceMode2D.Force);

        _rb.velocity = Vector2.zero;
    }
}