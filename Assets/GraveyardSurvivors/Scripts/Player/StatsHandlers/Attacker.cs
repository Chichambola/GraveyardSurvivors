using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _cooldown = 1.5f;

    private float _attackSpeed;
    private float _attackRadius;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    public void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }
    
    private IEnumerator AttackingCoroutine()
    {
        float currentCooldown = _cooldown;
        
        while (enabled)
        {
            currentCooldown = CompareCooldown(currentCooldown);
        
            var wait = new WaitForSeconds(currentCooldown);
            
            _weapon.Attack(currentCooldown, _attackRadius);

            yield return wait;
        }
    }

    private float CompareCooldown(float currentCooldown)
    {
        if (!Mathf.Approximately(currentCooldown, _cooldown))
        {
            _cooldown = UserUtils.SubtractPercentFromNumber(_cooldown, _attackSpeed);
            
            currentCooldown = _cooldown;
        }

        return currentCooldown;
    }
    
    private void OnStatsChanged(CharacterStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
        _attackRadius = stats.AttackRadius;
    }
    
    private void OnEnemyDetected(IAttacker attacker)
    {
        attacker.TakeDamage(_weapon.Info.Damage);
    }
}
