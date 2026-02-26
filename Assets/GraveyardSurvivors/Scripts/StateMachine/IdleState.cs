using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(CharacterBase character, Animator animator) : base(character, animator) { }

    public override void DoEnter()
    {
        Animator.CrossFade(s_Idle, CrossFadeDuration);
    }
}
