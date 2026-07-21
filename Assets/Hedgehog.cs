using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Hedgehog : Enemy, IFollower
{
    [SerializeField] private float _runawayDistance = 4f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Target _target;
    [SerializeField] private TextMeshProUGUI _running;
    [SerializeField] private TextMeshProUGUI _attacking;
    
    private bool _isRunningAway;
    private Vector3 _targetPosition;
    
    protected override void OnEnable()
    {
        base.OnEnable();

        _isRunningAway = false;
        
        _targetPosition = Vector3.zero;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _target.WasReached -= OnTargetReached;
    }

    public override void HandleMovement()
    {
        MoveTowards(_isRunningAway ? _targetPosition : Player.CurrentPosition);
    }

    protected override void OnAttackerDetected(IAttacker attacker, IWeapon weapon)
    {
        base.OnAttackerDetected(attacker, weapon);

        if (_isRunningAway)
            return;
        
        SetTargetPosition(attacker);
    }

    protected override void InitializeStateMachine()
    {
        var idleState = new IdleState(this, Animator);
        var dieState = new DieState(this, Animator);
        var runState = new RunState(this, Animator);
        var attackState = new EnemyAttackState(this, Animator);

        DefineAtTransition(idleState, runState, new FuncPredicate(() => IsAlive));
        
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentHealth <= 0));
        DefineAtTransition(runState, attackState, new FuncPredicate(() => CurrentHealth >= 0 && PlayerDetector.IsPlayerNear && !_isRunningAway));
        DefineAtTransition(attackState, runState, new FuncPredicate(() => CurrentHealth >= 0 && !PlayerDetector.IsPlayerNear || _isRunningAway));

        StateMachine.SetState(idleState);
    }
    
    private void SetTargetPosition(IAttacker attacker)
    {
        var target = attacker as MonoBehaviour;
        if (target == null)
            throw new Exception("Attacker is not MonoBehaviour");
        
        var direction = -(target.transform.position - gameObject.transform.position).normalized;

        var ray = new Ray(transform.position, direction);
        
        bool isHit = Physics.Raycast(ray, out RaycastHit hit, _runawayDistance, _layerMask);
        
        var position = isHit ? hit.collider.ClosestPointOnBounds(hit.transform.position) : ray.GetPoint(_runawayDistance);
        
        _targetPosition = new Vector3(position.x, 0, position.z);
        
        if (!_target.HasFollower)
        {
            _target = Instantiate(_target, _targetPosition, Quaternion.identity);
            _target.SetFollower(this);   
            _target.WasReached += OnTargetReached;
        }
        else
        {
            _target.SetPosition(_targetPosition);
        }
        
        _isRunningAway = true;
    }

    private void OnTargetReached()
    {
        _isRunningAway = false;
        _targetPosition = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (_isRunningAway)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, _targetPosition);
        }
    }
}
