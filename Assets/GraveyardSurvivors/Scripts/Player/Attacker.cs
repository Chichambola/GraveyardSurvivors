using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attacker : Stats<CharacterStats>
{
    [SerializeField] private Player _player;
    [SerializeField] private float _cooldown = 3f;

    private float _attackSpeed;
    private float _attackRadius;
    private Coroutine _coroutine;
    private IWeapon _weapon;
    
    protected override void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    protected override void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    public void SetWeapon(IWeapon weapon)
    {
        _weapon = weapon ?? throw new Exception();
    }
    
    public override void SetInitialStats(CharacterStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
        _attackRadius = stats.AttackRadius;
    }

    public void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }
    
    protected override void OnStatsChanged(CharacterStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
        _attackRadius = stats.AttackRadius;
    }
    
    private IEnumerator AttackingCoroutine()
    {
        _cooldown = UserUtils.SubstractPercentFromNumber(_cooldown, _attackSpeed);
        
        var wait = new WaitForSeconds(_cooldown);

        while (enabled)
        {
            _weapon.Attack();

            yield return wait;
        }
    }
}
