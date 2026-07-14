using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    public WalkState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }
    
    public override void DoEnter()
    {
        Animator.CrossFade(s_walk, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        StateHandler.HandleMovement();
    }
}
