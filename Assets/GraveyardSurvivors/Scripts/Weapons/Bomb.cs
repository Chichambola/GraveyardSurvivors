using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

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
}
