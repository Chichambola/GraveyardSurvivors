using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeWeapon : Weapon
{
    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
    }

    public override void Attack(float radiusMultiplier)
    {
        AttackStrategy.Execute(radiusMultiplier);
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