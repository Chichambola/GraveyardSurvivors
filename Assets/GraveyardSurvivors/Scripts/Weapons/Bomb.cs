using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using PrimeTween;
using Sirenix.Serialization;

public class Bomb : Weapon
{
    [SerializeField] private InterfaceReference<IAttacker, MonoBehaviour> _weaponBearer;
    [SerializeField] private float _damageAfterExplosion = 99999;
    
    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
        AttackStrategy.AttackExecuted += OnAttackExecuted;
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
        AttackStrategy.AttackExecuted -= OnAttackExecuted;
    }
    
    public override void Attack()
    {
        AttackStrategy.Execute();
    }

    public override void Reset()
    {
        AttackStrategy.Reset();
    }

    private void OnAttackerDetected(IAttacker attacker)
    {
        attacker.TakeDamage(Damage);
    }

    private void OnAttackExecuted()
    {
        _weaponBearer.Value.TakeDamage(Damage);
    }
}
