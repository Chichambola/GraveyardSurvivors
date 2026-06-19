using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeWeapon : Weapon
{
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    
    public override string UpgradeDescription { get; protected set; }
    
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
        _attackStrategy.Execute();
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