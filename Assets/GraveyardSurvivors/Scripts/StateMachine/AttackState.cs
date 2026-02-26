using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(CharacterBase character, Animator animator) : base(character, animator) { }

    public override void DoEnter()
    {
        Character.HandleAttack();
        
        Animator.CrossFade(s_Attack, CrossFadeDuration);
    }
}


