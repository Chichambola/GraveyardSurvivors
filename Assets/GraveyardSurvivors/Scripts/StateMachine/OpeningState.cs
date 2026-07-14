using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningState : BaseState
{
    public OpeningState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }
    
    public override void DoEnter()
    {
        Animator.CrossFade(s_opening, CrossFadeDuration);
    }
}
