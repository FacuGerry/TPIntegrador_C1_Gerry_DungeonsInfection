using UnityEngine;

public class RoydAnimations : MonoBehaviour
{
    private Animator _anim;
    private static readonly int state = Animator.StringToHash("State");

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        RoydMovement.OnAnimate += OnAnimate_Animate;
    }

    private void OnDisable()
    {
        RoydMovement.OnAnimate -= OnAnimate_Animate;
    }

    public void OnAnimate_Animate(int action)
    {
        _anim.SetInteger(state, action);
    }
}
