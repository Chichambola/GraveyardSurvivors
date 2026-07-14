using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class BaseState : IState
{
    protected readonly CharacterBase StateHandler;
    protected readonly Animator Animator;
    
    protected static readonly int s_run = Animator.StringToHash("Run");
    protected static readonly int s_idle = Animator.StringToHash("Idle");
    protected static readonly int s_attack = Animator.StringToHash("Attack");
    protected static readonly int s_death = Animator.StringToHash("Death");
    protected static readonly int s_walk = Animator.StringToHash("Walk");
    protected static readonly int s_opening = Animator.StringToHash("Opening");

    protected const float CrossFadeDuration = 0.15f;

    public BaseState(CharacterBase stateHandler, Animator animator)
    {
        StateHandler = stateHandler;
        Animator = animator;
    }
    
    public virtual void DoExit(){ }

    public virtual void DoEnter() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }
}
