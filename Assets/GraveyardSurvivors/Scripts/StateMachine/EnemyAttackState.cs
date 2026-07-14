using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class EnemyAttackState : BaseState
{
    public EnemyAttackState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }

    public override void DoEnter()
    {
        if (StateHandler is not Enemy character) return;
        
        Animator.CrossFade(s_attack, CrossFadeDuration);
        
        character.HandleAttack();
    }
}


