using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static event Action<CharacterDataSO, List<Character>> OnEnemyTurn;
    public static event Action<Character> OnEnemyClicked;
    public static event Action<Character> OnDataSet;

    public CharacterDataSO data;
    private SpriteRenderer _sprite;
    private Animator _animator;

    public int life;

    private bool _isSelectable;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _isSelectable = false;
    }

    private void OnMouseDown()
    {
        if (!_isSelectable)
            return;

        OnEnemyClicked?.Invoke(this);
    }

    public void SetData(CharacterDataSO dataChar)
    {
        data = dataChar;
        life = dataChar.life;
        _sprite = dataChar.sprite;
        _animator = dataChar.animator;

        gameObject.name = data.name;

        OnDataSet?.Invoke(this);
    }

    public void SetSelectable(bool isOn)
    {
        _isSelectable = isOn;
    }

    public void InCharacterTurn_Move(List<Character> players)
    {
        if (data.isPlayer)
            return;

        OnEnemyTurn?.Invoke(data, players);
    }
}