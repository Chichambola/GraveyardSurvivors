using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }

    public override void DoEnter()
    {
        Animator.CrossFade(s_Idle, CrossFadeDuration);
    }
}
