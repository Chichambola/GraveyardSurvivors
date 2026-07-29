using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;

public class Demon : Enemy
{
    [SerializeField] private InterfaceReference<IWeapon, MonoBehaviour> _weapon;

    private BaseState _idleState;

    protected override void Awake()
    {
        base.Awake();
        
        _idleState = new IdleState(this, Animator);
    }

    protected override void OnEnable()
    {
        _weapon.Value.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _weapon.Value.AttackerDetected -= OnAttackerDetected;
    }

    protected override void InitializeStateMachine()
    {
        base.InitializeStateMachine();
        
        DefineAnyTransition(_idleState, new FuncPredicate(() => PlayerDetector.IsPlayerNear));
    }

    protected override void Die()
    {
        base.Die();
        
        _weapon.Value.StopAttacking();
    }

    protected override void OnAttackerDetected(IAttacker attacker, IWeapon weapon)
    {
        weapon.Attack();
        StateMachine.SetState(_idleState);
    }
}
