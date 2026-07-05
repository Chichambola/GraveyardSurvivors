using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;

public class WeaponBow : WeaponWithAbility
{
    [SerializeField] private RangeAttackStrategy _attackStrategy;
    [SerializeField] private ProjectileSpawner _arrowSpawner;

    private Coroutine _attackingRoutine;
    private WaitForSeconds _attackTime;
    
    public override string UpgradeDescription { get; protected set; }

    public override void Init()
    {
        UpgradeDescription = $"Add +{BonusDamagePerUpgrade} damage \n" +
                             $"Add +{_attackStrategy.ProjectilePerUpgrade} to  total projectile amount.";
    }
    
    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
        _arrowSpawner.ProjectileReleased += ProcessAttacker;
        
        _attackTime = new WaitForSeconds(Cooldown);
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
        _arrowSpawner.ProjectileReleased -= ProcessAttacker;
    }
    
    public override void Attack()
    {
        _attackStrategy.Execute();
    }

    public override void Upgrade()
    {
        _attackStrategy.Upgrade();
        
        base.Upgrade();
    }

    public override void StartAttacking()
    {
        if (_attackingRoutine != null)
            StopCoroutine(_attackingRoutine);

        _attackingRoutine = StartCoroutine(AttackRoutine());
    }

    public override void SetCooldown(float cooldown)
    {
        base.SetCooldown(cooldown);
        
        _attackTime = new WaitForSeconds(cooldown);
    }

    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker is Enemy enemy)
        {
            _arrowSpawner.Spawn(enemy);  
        }
    }

    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        
        while (enabled)
        {
            yield return _attackTime;
            
            _attackStrategy.Execute();
            
            IsAttacking = false;
        }
    }
}
