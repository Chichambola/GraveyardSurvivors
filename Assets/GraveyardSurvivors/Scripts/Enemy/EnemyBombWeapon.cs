using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombWeapon : Weapon
{
    [SerializeField] private float _explosionRadius;

    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
    }

    public override void Attack(float duration)
    {
        AttackStrategy.Execute(_explosionRadius, duration);
    }
    
    private void OnAttackerDetected(IAttacker attacker)
    {
        attacker.TakeDamage(Info.Damage);
    }
}
