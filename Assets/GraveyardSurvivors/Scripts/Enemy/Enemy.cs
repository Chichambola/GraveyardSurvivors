using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>
{
    [SerializeField] private bool _isRunning = true;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private EnemyInfo _info;
    [Header("Weapon")]
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _attackCooldown = 0.5f;
    [Header("Services")]
    [SerializeField] private Defender _defender;
    [SerializeField] private TextMeshProUGUI _health;

    public event Action<Enemy> CanBeReleased;

    private Player _player;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private List<IEffect<IAttacker>> _currentEffects;
    private float _initialHealth;
    private float _initialSpeed;
    private int _movementEffectCount;

    public EnemyStats CurrentStats { get; private set; }
    public Rigidbody Rigidbody => _rigidbody;
    public float Damage => _weapon.Info.Damage;
    public float Speed => Mover.Speed;

    public void Init(Player player)
    {
        _player = player;
    }

    protected override void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentEffects = new List<IEffect<IAttacker>>();
        
        CurrentStats = _info.GetStats();
        
        _initialHealth = CurrentStats.Health;
        _initialSpeed = CurrentStats.MovementSpeed;
    }

    protected override void Update()
    {
        StateMachine.Update();
    }

    protected override void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
        
        InitializeStateMachine();
        
        _health.text = $"{_initialHealth:f1}";
    }

    public void ResetCharacteristics()
    {
        CurrentStats.Health = _initialHealth;
        CurrentStats.MovementSpeed = _initialSpeed;
    }

    public void Release() => CanBeReleased?.Invoke(this);

    public void TakeDamage(float damage)
    {
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);

        CurrentStats.Health -= damage;

        _health.text = $"{CurrentStats.Health:f1}";
        
        if (CurrentStats.Health <= 0)
        {
            Die();
        }
    }
    
    public void ApplyEffect(IEffect<IAttacker> effect)
    {
        if (effect is IMovementEffect)
        {
            _movementEffectCount++;
        }
        
        effect.EffectCompleted += RemoveEffect;
        _currentEffects.Add(effect);
        effect.Apply(this);
    }
    
    public void ChangeSpeed(float speedValue, bool isSlowing)
    {
        float tempSpeed;
        
        if (isSlowing)
        {
            tempSpeed = UserUtils.SubtractPercentFromNumber(Mover.Speed, speedValue); 
        }
        else
        {
            tempSpeed = UserUtils.AddPercentToNumber(Mover.Speed, speedValue); 
        }

        if (_movementEffectCount == 0)
        {
            Mover.ResetSpeed();
        }
        else
        {
            Mover.SetSpeed(tempSpeed);   
        }
    }

    public override void HandleMovement()
    {
        Mover.Move(_player.transform, CurrentStats.MovementSpeed);

        Vector3 direction = UserUtils.GetDirection(_player.transform.position, transform.position);

        Rotator.Rotate(direction);
    }

    public override void HandleAttack()
    {
        _weapon.Attack(_attackCooldown);
    }

    public void Die()
    {
        RemoveAllEffects();
        
        CurrentStats.Health = 0;
        
        Mover.ResetSpeed();
            
        _movementEffectCount = 0;

        _health.text = $"{CurrentStats.Health}";
    }
    
    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        var attackState = new EnemyAttackState(this, Animator);
        var dieState = new DieState(this, Animator);
        var walkState = new WalkState(this, Animator);

        DefineAtTransition(idleState, runState, new FuncPredicate(() => Mover.Speed > 0 && _isRunning));
        DefineAtTransition(idleState, walkState, new FuncPredicate(() => Mover.Speed > 0 && !_isRunning));
        DefineAtTransition(attackState, runState,
            new FuncPredicate(() => !_playerDetector.IsPlayerNear && !_weapon.IsAttacking && _isRunning));
        DefineAtTransition(attackState, walkState,
            new FuncPredicate(() => !_playerDetector.IsPlayerNear && !_weapon.IsAttacking && !_isRunning));
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentStats.Health <= 0));
        DefineAnyTransition(attackState, new FuncPredicate(() => CurrentStats.Health >= 0 && _playerDetector.IsPlayerNear));

        StateMachine.SetState(idleState);
    }

    private void RemoveAllEffects() 
    {
        if (_currentEffects.Count > 0)
        {
            for (int i = _currentEffects.Count - 1; i >= 0; i--)
            {
                var effect = _currentEffects[i];
                
                effect.EffectCompleted -= RemoveEffect;

                effect.Cancel();
            }
        }
    }
    
    private void RemoveEffect(IEffect<IAttacker> effect)
    {
        if (effect is IMovementEffect)
        {
            _movementEffectCount--;
            
            if (_movementEffectCount == 0)
            {
                Mover.ResetSpeed();
            }
        }
        
        effect.EffectCompleted -= RemoveEffect;
        _currentEffects.Remove(effect);
    }
}