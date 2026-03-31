using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSkeletonArm : Weapon
{
    private bool _isAttacking;
    private Coroutine _coroutine;
    private float _duration;

    public override bool IsAttacking => _isAttacking;

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
        _isAttacking = true;
        _duration = duration;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingRoutine());
    }

    private IEnumerator AttackingRoutine()
    {
        var wait = new WaitForSecondsRealtime(_duration);

        while (enabled)
        {
            yield return wait;

            AttackStrategy.Execute();

            _isAttacking = false;
        }
    }

    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker == null)
            throw new Exception();
        
        if (attacker is Player player)
        {
            player.TakeDamage(Info.Damage);
        }
    }
}