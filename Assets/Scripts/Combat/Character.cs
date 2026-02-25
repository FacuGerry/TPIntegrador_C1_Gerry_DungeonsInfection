using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Character : MonoBehaviour
{
    public static event Action<Character, List<Character>> OnEnemyTurn;
    public static event Action<Character> OnDataSet;
    public static event Action<Character> OnInfected;

    public CharacterDataSO data;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private static readonly int _state = Animator.StringToHash("State");

    public int life;
    [NonSerialized] public int maxLife;
    [NonSerialized] public bool isMagicShieldOn = false;
    [NonSerialized] public bool isAbsorbOn = false;
    [NonSerialized] public int defense = 0;
    [NonSerialized] public bool isInfected = false;

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

        CombatManager.OnRoundEnd += OnRoundEnd_Infect;

        CombatManager.OnSettingLifeToLevel += SetLifeToLevel;

        LevelsSystem.OnLevelUp += OnLevelUp_AddStats;
    }

    private void OnDisable()
    {
        CombatManager.OnRoundEnd -= OnRoundEnd_ResetDefense;

        CombatManager.OnRoundEnd -= OnRoundEnd_Infect;

        CombatManager.OnSettingLifeToLevel -= SetLifeToLevel;

        LevelsSystem.OnLevelUp -= OnLevelUp_AddStats;
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
        _animator.SetInteger(_state, (int)action);
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
        isMagicShieldOn = false;
        isAbsorbOn = false;
    }

    public void OnRoundEnd_Infect()
    {
        if (!data.isPlayer && isInfected)
        {
            life -= data.lifeToQuitOnInfected;
            OnInfected?.Invoke(this);
        }
    }

    public void SetLifeToLevel(Character enemy, BattleDefinitionSO battle)
    {
        if (!enemy.data.isPlayer)
        {
            enemy.life *= (battle.battleLevel / 10) + 1;
        }
    }

    public void OnLevelUp_AddStats(int level, int levelBefore)
    {
        if (!data.isPlayer)
            return;

        for (int i = levelBefore; i < level; i++)
        {
            data.attack += data.valueToAddWhenLevelUp;
            data.life += data.valueToAddWhenLevelUp;

            data.defense += data.valueToAddWhenLevelUpDefense;

            data.fireball += data.valueToAddWhenLevelUp;

            data.magicShield += data.valueToAddWhenLevelUpDefense;
            data.absorb += data.valueToAddWhenLevelUpDefense;
            data.heal += data.valueToAddWhenLevelUpDefense;
        }
    }
}