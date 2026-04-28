using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : BaseState
{
    public RunState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }

    public override void DoEnter()
    {
        Animator.CrossFade(s_Run, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        StateHandler.HandleMovement();
    }
}
