using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static event Action<CharacterDataSO, List<Character>> OnPlayerTurn;
    public static event Action<CharacterDataSO, List<Character>> OnEnemyTurn;

    [SerializeField] private CharacterDataSO _data;
    private SpriteRenderer _sprite;
    private Animator _animator;

    public int life;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetData(CharacterDataSO data)
    {
        _data = data;
        life = data.life;
        _sprite = data.sprite;
        _animator = data.animator;

        gameObject.name = _data.name;
    }

    public void InCharacterTurn_Move(List<Character> players, List<Character> enemies)
    {
        if (_data.isPlayer)
            OnPlayerTurn?.Invoke(_data, enemies);
        else
            OnEnemyTurn?.Invoke(_data, players);
    }
}