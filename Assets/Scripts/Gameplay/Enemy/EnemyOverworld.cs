using UnityEngine;

public class EnemyOverworld : MonoBehaviour
{
    private static readonly int _state = Animator.StringToHash("State");

    private Animator _anim;

    public enum States
    {
        IdleUp = 0,
        IdleDown,
        IdleSide,
    }

    [SerializeField] private States _animationState;
    private States _animState;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_animState != _animationState)
        {
            _animState = _animationState;
            _anim.SetInteger(_state, (int)_animState);
        }
    }
}
