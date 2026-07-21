using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BasicWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _damagePerUpgrade = 1;
    
    private Coroutine _coroutine;
    private bool _isAttacking;
    
    public event Action<IAttacker, IWeapon> AttackerDetected;

    public float Damage => _damage;
    public bool IsAttacking => _isAttacking;
    
    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
    }
    
    public void Upgrade()
    {
        _damage += _damagePerUpgrade;
    }
    
    public void Attack()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackRoutine());
    }
    
    public void StopAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
    
    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        
        var wait = new WaitForSeconds(_cooldown);
        
        while (enabled)
        {
            yield return wait;
            
            _attackStrategy.Execute();
            
            _isAttacking = false;
        }
    }
    
    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker == null)
            throw new Exception();
        
        AttackerDetected?.Invoke(attacker, this);
    }
}
