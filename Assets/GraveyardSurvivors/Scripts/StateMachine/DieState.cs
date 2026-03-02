using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    public DieState(CharacterBase character, Animator animator) : base(character, animator) { }

    public override void DoEnter()
    {
        Animator.CrossFade(s_Death, CrossFadeDuration);
    }
}
