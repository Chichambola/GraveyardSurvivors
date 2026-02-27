using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackerHandler : AttackerHandlerBase<EnemyStats>
{
    private IWeapon _weapon;
    private Coroutine _coroutine;
    private float _attackSpeed;
    
    public override void SetWeapon(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public override void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }

    public override void OnEnemyDetected(IAttacker attacker)
    {
        attacker.TakeDamage(_weapon.Info.Damage);
    }

    public override void UpdateStats(EnemyStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
    }

    protected override IEnumerator AttackingCoroutine()
    {
        throw new System.NotImplementedException();
    }
}
