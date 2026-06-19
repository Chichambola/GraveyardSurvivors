using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBow : WeaponWithAbility
{
    [SerializeField] private RangeAttackStrategy _attackStrategy;
    [SerializeField] private ProjectileSpawner _arrowSpawner;

    public override string UpgradeDescription { get; protected set; }

    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
        _arrowSpawner.ProjectileReleased += ProcessAttacker;
        UpgradeDescription = $"Add +{BonusDamage} damage \n" +
                             $"Add +{_attackStrategy.ProjectilePerUpgrade} to  total projectile amount.";
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

    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker is Enemy enemy)
        {
            _arrowSpawner.Spawn(enemy);  
        }
    }
}
