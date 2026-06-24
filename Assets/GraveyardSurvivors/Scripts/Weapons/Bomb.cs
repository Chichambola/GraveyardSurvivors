using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using PrimeTween;
using Sirenix.Serialization;

public class Bomb : Weapon
{
    [SerializeField] private BombAttackStrategy _attackStrategy;
    [SerializeField] private InterfaceReference<IAttacker, MonoBehaviour> _weaponBearer;
    [SerializeField] private float _damageAfterExplosion = 99999;
    
    public override string UpgradeDescription { get; protected set; }
    
    public override void Init()
    {
        throw new NotImplementedException();
    }
    
    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
        _attackStrategy.AttackExecuted += OnAttackExecuted;
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
        _attackStrategy.AttackExecuted -= OnAttackExecuted;
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

    public override void Reset()
    {
        _attackStrategy.Reset();
    }

    public override void StopAttacking()
    {
        _attackStrategy.Stop();
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
