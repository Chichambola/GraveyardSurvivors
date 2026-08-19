using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Hedgehog : Enemy, IFollower
{
    [Header("Follower settings")]
    [SerializeField] private float _runawayDistance = 4f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Target _target;
    [Header("Upgrade")]
    [SerializeField] private float _upgradeDamage = 2f;
    
    private bool _isRunningAway;
    private Vector3 _targetPosition;
    
    protected override void OnEnable()
    {
        InitializeStateMachine();

        PlayerDetector.PlayerDetected += OnPlayerDetected;

        _isRunningAway = false;
        
        _targetPosition = Vector3.zero;
    }

    protected void OnDisable()
    {
        PlayerDetector.PlayerDetected -= OnPlayerDetected;
        _target.WasReached -= OnTargetReached;
    }

    public override void Upgrade(EnemyStats stats)
    {
        base.Upgrade(stats);

        DamageZone.Upgrade(_upgradeDamage);
    }
    
    public override void HandleMovement()
    {
        MoveTowards(_isRunningAway ? _targetPosition : Player.CurrentPosition);
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
    
    private void OnPlayerDetected(IPlayer player)
    {
        if (_isRunningAway)
            return;
        
        SetTargetPosition(player as IAttacker);
    }
}
