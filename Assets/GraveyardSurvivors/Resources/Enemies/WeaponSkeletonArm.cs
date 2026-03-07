using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkeletonArm : Weapon
{
    [SerializeField] private AttackArea _area;

    public event Action<Weapon> FinishedAttacking;

    private Coroutine _coroutine;
    
    public override void Attack(float duration)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingRoutine(duration));
    }

    private IEnumerator AttackingRoutine(float duration)
    {
        var wait = new WaitForSecondsRealtime(duration);

        while (enabled)
        {
            yield return wait;

            if (_area.TryGetAttacker(out IAttacker attacker))
            {
                attacker.TakeDamage(_info.Damage);
            }
            else
            {
                FinishedAttacking?.Invoke(this);
            }
        }
    }

    public override void StopAttacking()
    {
        StopCoroutine(_coroutine);
    }
}
