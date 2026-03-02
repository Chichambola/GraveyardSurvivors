using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _cooldown = 1.5f;

    private float _attackSpeed;
    private float _attackRadius;
    private Coroutine _coroutine;
    
    public bool IsAttacking { get; private set; }

    public void Attack(float duration)
    {
        IsAttacking = true;
        
        _weapon.Attack(duration);
    }
    
    public void StartAttacking(float attackSpeedMultiplier, float attackRadiusMultiplier)
    {
        _attackSpeed = attackSpeedMultiplier;
        _attackRadius = attackRadiusMultiplier;
        
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

    public void OnEnemyDetected(IAttacker attacker)
    {
        attacker.TakeDamage(_weapon.Info.Damage);
    }
}
