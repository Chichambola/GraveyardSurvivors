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
        Animator.CrossFade(s_attack, CrossFadeDuration);
    }

    public override void Update()
    {
        StateHandler.HandleAttack();
    }
}


