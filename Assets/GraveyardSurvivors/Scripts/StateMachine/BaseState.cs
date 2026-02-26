using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : IState
{
    protected readonly CharacterBase Character;
    protected readonly Animator Animator;

    protected static readonly int s_Run = Animator.StringToHash("Run");
    protected static readonly int s_Idle = Animator.StringToHash("Idle");
    protected static readonly int s_Attack = Animator.StringToHash("Attack");

    protected const float CrossFadeDuration = 0.1f;

    public BaseState(CharacterBase character, Animator animator)
    {
        Character = character;
        Animator = animator;
    }
    
    public virtual void DoExit()
    {
        // noop
    }

    public virtual void DoEnter()
    {
        // noop
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void Update()
    {
        // noop
    }
}
