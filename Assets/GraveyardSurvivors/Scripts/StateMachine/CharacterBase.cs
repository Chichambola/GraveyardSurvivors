using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rotator), typeof(Mover))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class CharacterBase : MonoBehaviour, IStateHandler
{
    [Header("Base")]
    [SerializeField] protected Animator Animator;
    [SerializeField] protected Rotator Rotator;
    [SerializeField] protected Mover Mover;
    
    protected CapsuleCollider Collider;
    protected StateMachine StateMachine;
    
    protected abstract void Awake();
    
    protected abstract void Update();

    protected abstract void FixedUpdate();

    public abstract void HandleMovement();

    public virtual void HandleAttack() { }

    public abstract void Release();

    protected void DefineAtTransition(IState from, IState to, IPredicate condition)
    {
        StateMachine.AddTransition(from, to, condition);
    }

    protected void DefineAnyTransition(IState to, IPredicate condition)
    {
        StateMachine.AddAnyTransition(to, condition);
    }

    protected abstract void InitializeStateMachine();
}
