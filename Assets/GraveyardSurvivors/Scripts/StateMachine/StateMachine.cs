using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class StateMachine
{
    private StateNode _current;

    private Dictionary<Type, StateNode> _nodes = new();
    private HashSet<ITransition> _anyTransitions = new();

    public void Update()
    {
        var transition = GetTransition();

        if (transition != null)
            ChangeState(transition.To);
        
        _current.State?.Update();
    }

    public void FixedUpdate()
    {
        _current.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        _current = _nodes[state.GetType()];
        _current.State?.DoEnter();
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAdd(from).AddTransition(GetOrAdd(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        _anyTransitions.Add(new Transition(to, condition));
    }
    
    private StateNode GetOrAdd(IState state)
    {
        var node = _nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
        }

        return node;
    }
    
    private void ChangeState(IState state)
    {
        if (state == _current.State)
            return;

        var previousState = _current.State;
        var nextState = _nodes[state.GetType()].State;
        
        previousState?.DoExit();
        nextState?.DoEnter();
        _current = _nodes[state.GetType()];
    }

    private ITransition GetTransition()
    {
        foreach (var transition in _anyTransitions)
        {
            if (transition.Condition.Evaluate())
                return transition;
        }

        foreach (var transition in _current.Transitions)
        {
            if (transition.Condition.Evaluate())
                return transition;
        }

        return null;
    }
}