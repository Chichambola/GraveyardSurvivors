using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
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
        attacker.TakeDamage(Info.Damage);
    }
}
