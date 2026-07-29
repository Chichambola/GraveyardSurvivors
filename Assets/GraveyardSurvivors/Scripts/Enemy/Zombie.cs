using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField] private InterfaceReference<IWeapon, MonoBehaviour> _weapon;
    
    private EnemyAttackState _attackState;
    private FuncPredicate _s;

    protected override void Awake()
    {
        base.Awake();
        
        _attackState = new EnemyAttackState(this, Animator);
    }

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
        
        DefineAtTransition(RunState, _attackState, new FuncPredicate(() => CurrentHealth >= 0 && PlayerDetector.IsPlayerNear));
        DefineAtTransition(_attackState, RunState, new FuncPredicate(() => CurrentHealth >= 0 && !PlayerDetector.IsPlayerNear && !_weapon.Value.IsAttacking));
    }

    protected override void Die()
    {
        base.Die();
        
        _weapon.Value.StopAttacking();
    }
}
