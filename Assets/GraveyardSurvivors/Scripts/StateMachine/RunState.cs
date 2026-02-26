using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : BaseState
{
    public RunState(CharacterBase character, Animator animator) : base(character, animator) { }

    public override void DoEnter()
    {
        Animator.CrossFade(s_Run, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        Character.HandleMovement();
    }
}
