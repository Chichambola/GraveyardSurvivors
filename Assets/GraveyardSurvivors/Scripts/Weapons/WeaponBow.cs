using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBow : WeaponWithAbility
{
    [SerializeField] private ProjectileSpawner _arrowSpawner;

    private int _damageUpgrade = 1;
    
    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
        _arrowSpawner.ProjectileReleased += ProcessAttacker;
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
        _arrowSpawner.ProjectileReleased -= ProcessAttacker;
    }

    public override void Attack(float radiusMultiplier)
    {
        AttackStrategy.Execute(radiusMultiplier);
    }

    public override void Upgrade()
    {
        AttackStrategy.Upgrade();
        BonusDamage += _damageUpgrade;
    }

    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker is Enemy enemy)
        {
            _arrowSpawner.Spawn(enemy);  
        }
    }
}
