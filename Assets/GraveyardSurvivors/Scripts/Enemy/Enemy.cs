using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>
{
    [SerializeField] private EnemyInfo _info;
    [SerializeField] private PlayerDetector _playerDetector;
    
    public event Action<Enemy> CanBeReleased;

    private EnemyStats _currentStats;
    private Player _player;

    public void Init(Player player)
    {
        _player = player;
    }

    protected override void Awake()
    {
        _currentStats = _info.GetStats();
        
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        var attackState = new AttackState(this, Animator);
        
        DefineAtTransition(idleState, runState, new FuncPredicate((() => _player != null)));
        DefineAtTransition(runState, attackState, new FuncPredicate((() => _playerDetector.IsPlayerNear)));
        DefineAtTransition(attackState, runState, new FuncPredicate((() => !_playerDetector.IsPlayerNear)));
        
        StateMachine.SetState(idleState);
    }

    protected override void Update()
    {
        StateMachine.Update();
    }

    protected override void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    public void ResetCharacteristics()
    {
        _player = null;
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
    
    public void TakeDamage(float damage)
    {
        throw new NotImplementedException();
    }

    public override void HandleMovement()
    {
        Mover.MoveTowardsTarget(_player.transform, _currentStats.MovementSpeed);
        
        Vector3 direction = UserUtils.GetDirection(_player.transform.position, transform.position);
        
        Rotator.Rotate(direction);
    }

    public override void HandleAttack()
    {
        
    }
}
