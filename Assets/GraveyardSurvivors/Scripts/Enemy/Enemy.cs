using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>
{
    [SerializeField] private DamageZone _damageZone;
    [Header("Weapon")]
    [SerializeField] protected Weapon Weapon;
    [SerializeField] private float _attackCooldown = 0.5f;
    [Header("Services")]
    [SerializeField] private Defender _defender;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] protected PlayerDetector PlayerDetector;

    public event Action<Enemy> CanBeReleased;
    public event Action<Enemy> NoHealthLeft;
    
    private IPlayer _player;
    private Coroutine _attackRoutine;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private List<IEffect<IAttacker>> _currentEffects;
    private float _initialHealth;
    private float _initialSpeed;
    private int _movementEffectCount;
    private bool _isAttacking;

    public EnemyStats CurrentStats { get; private set; }
    public bool IsAlive { get; private set; }
    public float CurrentHealth { get; private set; }
    public Vector3 CurrentPosition => transform.position;

    public void Init(IPlayer player, EnemyStats stats)
    {
        _player = player;

        CurrentStats = stats; 
        
        CurrentHealth = stats.MaxHealth;
        
        _health.text = $"{CurrentStats.MaxHealth:f1}";
    }

    protected override void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentEffects = new List<IEffect<IAttacker>>();
        
        StateMachine = new StateMachine();
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
        InitializeStateMachine();

        IsAlive = true;
    }

    public void ResetCharacteristics()
    {
        IsAlive = true;
    }

    public override void Release() => CanBeReleased?.Invoke(this);
    
    public void SetStats(EnemyStats stats)
    {
        CurrentStats = stats;
    }

    public void TakeDamage(float damage)
    {
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);

        CurrentHealth -= damage;

        _health.text = $"{CurrentHealth:f1}";
        
        if (CurrentHealth <= 0 && IsAlive)
        {
            IsAlive = false;
            
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
            tempSpeed = Mover.Speed.SubtractPercentFromNumber(speedValue); 
        }
        else
        {
            tempSpeed = Mover.Speed.AddPercentToNumber(speedValue); 
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
        Mover.MoveToPosition(_player.CurrentPosition, CurrentStats.MovementSpeed);
        
        Vector3 direction = (_player.CurrentPosition - transform.position).normalized;
        
        Rotator.Rotate(direction);
    }

    public override void HandleAttack()
    {
        if(_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        _attackRoutine = StartCoroutine(AttackRoutine());
    }
    
    public void SetColliderCenter(Vector3 offsetAfterDeath, bool isResetting)
    {
        if (isResetting)
        {
            _collider.center -= offsetAfterDeath;
            _damageZone.gameObject.transform.position -= offsetAfterDeath;
        }
        else
        {
            _collider.center += offsetAfterDeath;
            _damageZone.gameObject.transform.position += offsetAfterDeath;
        }
    }
    
    protected override void InitializeStateMachine()
    {
        var idleState = new IdleState(this, Animator);
        var dieState = new DieState(this, Animator);
        var runState = new RunState(this, Animator);
        var attackState = new EnemyAttackState(this, Animator);

        DefineAtTransition(idleState, runState, new FuncPredicate(() => IsAlive));
        
        DefineAtTransition(attackState, runState,
            new FuncPredicate(() => !PlayerDetector.IsPlayerNear && !_isAttacking));
        
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentHealth <= 0));
        DefineAnyTransition(attackState, new FuncPredicate(() => CurrentHealth >= 0 && PlayerDetector.IsPlayerNear));

        StateMachine.SetState(idleState);
    }
    
    private void Die()
    {
        NoHealthLeft?.Invoke(this);
        
        CurrentHealth = 0;
        
        _movementEffectCount = 0;

        _health.text = $"{CurrentHealth}";
        
        Weapon.StopAttacking();
        
        RemoveAllEffects();
        
        Mover.ResetSpeed();
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

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        
        var wait = new WaitForSecondsRealtime(_attackCooldown);

        while (enabled)
        {
            yield return wait;
            
            Weapon.Attack();
            
            _isAttacking = false;
        }
    }
}