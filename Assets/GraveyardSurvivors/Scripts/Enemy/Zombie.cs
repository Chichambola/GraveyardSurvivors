using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    protected override void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var idleState = new IdleState(this, Animator);
        var dieState = new DieState(this, Animator);
        var walkState = new WalkState(this, Animator);
        var attackState = new EnemyAttackState(this, Animator);
        
        DefineAtTransition(idleState, walkState, new FuncPredicate(() => Mover.Speed > 0));
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentStats.Health <= 0));
        DefineAnyTransition(attackState, new FuncPredicate(() => CurrentStats.Health >= 0 && PlayerDetector.IsPlayerNear));
        
        StateMachine.SetState(idleState);
    }
}
