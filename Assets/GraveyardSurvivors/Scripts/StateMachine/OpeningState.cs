using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningState : BaseState
{
    public OpeningState(IStateHandler stateHandler, Animator animator) : base(stateHandler, animator) { }
    
    public override void DoEnter()
    {
        Animator.CrossFade(s_Opening, CrossFadeDuration);
    }
}
