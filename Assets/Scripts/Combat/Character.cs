using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static event Action<Character, List<Character>> OnEnemyTurn;
    public static event Action<Character> OnDataSet;

    public enum CharacterStates
    {
        None = -1,
        Idle,
        Attack,
        Hurt,
        Dead
    }

    public CharacterDataSO data;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private static readonly int state = Animator.StringToHash("State");

    public int life;
    [NonSerialized] public int maxLife;
    [NonSerialized] public bool isDarkShieldOn = false;
    [NonSerialized] public int defense = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(7, 7, 7);
    }

    private void OnEnable()
    {
        CombatManager.OnRoundEnd += OnRoundEnd_ResetDefense;
    }

    private void OnDisable()
    {
        CombatManager.OnRoundEnd -= OnRoundEnd_ResetDefense;
    }

    public void SetData(CharacterDataSO dataChar)
    {
        data = dataChar;
        life = dataChar.life;
        maxLife = life;
        _spriteRenderer.sprite = dataChar.spriteRenderer.sprite;
        _animator.runtimeAnimatorController = dataChar.animator.runtimeAnimatorController;

        gameObject.name = data.name;

        OnDataSet?.Invoke(this);
    }

    public void Animate(CharacterStates action)
    {
        _animator.SetInteger(state, (int)action);
    }

    public void InCharacterTurn_Move(List<Character> players)
    {
        if (data.isPlayer)
            return;

        OnEnemyTurn?.Invoke(this, players);
    }

    public void OnRoundEnd_ResetDefense()
    {
        defense = 0;
        isDarkShieldOn = false;
    }
}