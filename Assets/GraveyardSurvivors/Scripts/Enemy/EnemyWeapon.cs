using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    private Coroutine _coroutine;
    private float _duration;

    public override bool IsAttacking { get; protected set; }

    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
        
        IsAttacking = false;
    }

    public override void Attack(float duration)
    {
        IsAttacking = true;
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

            IsAttacking = false;
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