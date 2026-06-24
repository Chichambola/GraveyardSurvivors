using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeWeapon : Weapon
{
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    
    private Coroutine _coroutine;
    
    public override string UpgradeDescription { get; protected set; }

    public override void Init()
    {
        
    }

    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
    }
    
    public override void Attack()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackRoutine());
    }
    
    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        
        var wait = new WaitForSeconds(Cooldown);
        
        while (enabled)
        {
            yield return wait;
            
            _attackStrategy.Execute();
            
            IsAttacking = false;
        }
    }
    
    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker == null)
            throw new Exception();
        
        if (attacker is Player player)
        {
            player.TakeDamage(Damage);
        }
    }
}