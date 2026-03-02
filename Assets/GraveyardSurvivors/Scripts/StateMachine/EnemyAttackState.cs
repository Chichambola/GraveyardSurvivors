using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class EnemyAttackState : BaseState
{
    public EnemyAttackState(CharacterBase character, Animator animator) : base(character, animator) { }

    public override void DoEnter()
    {
        Animator.CrossFade(s_Attack, CrossFadeDuration);
        
        Character.HandleAttack();
    }
}


