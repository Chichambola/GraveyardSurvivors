using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    private BaseState _idleState;

    public override void HandleAttack()
    {
        StateMachine.SetState(_idleState);
        
        base.HandleAttack();
    }

    protected override void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        _idleState = new IdleState(this, Animator);
        var dieState = new DieState(this, Animator);
        var runState = new RunState(this, Animator);
        var attackState = new EnemyAttackState(this, Animator);
        
        DefineAtTransition(_idleState, runState, new FuncPredicate(() => Mover.Speed > 0));
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentHealth <= 0));
        DefineAnyTransition(attackState, new FuncPredicate(() => CurrentHealth >= 0 && PlayerDetector.IsPlayerNear));
        
        StateMachine.SetState(_idleState);
    }
}
