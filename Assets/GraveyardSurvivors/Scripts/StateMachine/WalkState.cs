using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    public WalkState(IStateHandler stateHandler, Animator animator) : base(stateHandler, animator) { }
    
    public override void DoEnter()
    {
        Animator.CrossFade(s_Walk, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        if (StateHandler is not CharacterBase character) return;
        
        character.HandleMovement();
    }
}
