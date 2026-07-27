using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private InterfaceReference<IWeapon, MonoBehaviour> _weapon;

    protected override void OnEnable()
    {
        base.OnEnable();

        _weapon.Value.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _weapon.Value.AttackerDetected -= OnAttackerDetected;
    }

    public override void Upgrade(EnemyStats stats)
    {
        base.Upgrade(stats);

        _weapon.Value.Upgrade();
    }

    public override void HandleAttack()
    {
        _weapon.Value.Attack();
    }

    protected override void InitializeStateMachine()
    {
        base.InitializeStateMachine();

        var attackState = new EnemyAttackState(this, Animator);
        
        DefineAnyTransition(attackState, new FuncPredicate(() => CurrentHealth >= 0 && PlayerDetector.IsPlayerNear));
    }

    protected override void Die()
    {
        base.Die();
        
        _weapon.Value.StopAttacking();
    }
}
