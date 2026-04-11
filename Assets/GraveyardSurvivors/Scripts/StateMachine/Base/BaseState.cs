using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class BaseState : IState
{
    protected readonly CharacterBase Character;
    protected readonly Animator Animator;
    
    protected static readonly int s_Run = Animator.StringToHash("Run");
    protected static readonly int s_Idle = Animator.StringToHash("Idle");
    protected static readonly int s_Attack = Animator.StringToHash("Attack");
    protected static readonly int s_Death = Animator.StringToHash("Death");
    protected static readonly int s_Walk = Animator.StringToHash("Walk");

    protected const float CrossFadeDuration = 0.1f;

    public BaseState(CharacterBase character, Animator animator)
    {
        Character = character;
        Animator = animator;
    }
    
    public virtual void DoExit(){ }

    public virtual void DoEnter() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }
}
