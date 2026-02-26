using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected Animator Animator;
    [SerializeField] protected Rotator Rotator;
    [SerializeField] protected Mover Mover;
    
    protected StateMachine StateMachine;

    protected abstract void Awake();
    
    protected abstract void Update();

    protected abstract void FixedUpdate();

    public abstract void HandleMovement();

    public virtual void HandleAttack() { }

    protected void DefineAtTransition(IState from, IState to, IPredicate condition)
    {
        StateMachine.AddTransition(from, to, condition);
    }

    protected void DefineAnyTransition(IState to, IPredicate condition)
    {
        StateMachine.AddAnyTransition(to, condition);
    }
}
