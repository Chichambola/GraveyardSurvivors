using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBow : Weapon
{
    [SerializeField] private ProjectileSpawner _arrowSpawner;
    [SerializeField] private Effect[] _ivyEffect;

    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
    }

    public override void Attack(float duration, float radius)
    {
        AttackStrategy.Execute(radius);
    }
    
    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker is Enemy enemy)
        {
            _arrowSpawner.Spawn(enemy, Info.Damage);   
        }
    }
}
