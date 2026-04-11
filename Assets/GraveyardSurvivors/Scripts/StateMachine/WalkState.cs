using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    public WalkState(CharacterBase character, Animator animator) : base(character, animator) { }
    
    public override void DoEnter()
    {
        Animator.CrossFade(s_Walk, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        Character.HandleMovement();
    }
}
