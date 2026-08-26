using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>, ITarget
{
    [SerializeField] protected DamageZone DamageZone;
    
    [Header("Services")]
    [SerializeField] protected PlayerDetector PlayerDetector;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private Defender _defender;

    [Header("Collider values")]
    [SerializeField] private Vector3 _colliderSize;
    [SerializeField] private Vector3 _center;
    
    public event Action<Enemy> CanBeReleased;
    public event Action<Enemy> NoHealthLeft;
    public event Action<Enemy> TookDamage; 
    
    protected IPlayer Player;
    protected IdleState IdleState;
    protected RunState RunState;
    private Coroutine _attackRoutine;
    private Rigidbody _rigidbody;
    private List<IEffect<IAttacker>> _currentEffects;
    private float _currentHealth;
    private float _storedDamage;
    private int _movementEffectCount;
    private DieState _dieState;

    public EnemyStats CurrentStats { get; private set; }
    public bool IsAlive { get; private set; }
    public float CurrentHealth => _currentHealth - _storedDamage;
    public float MaxHealth => CurrentStats.MaxHealth;
    public Vector3 CurrentPosition => transform.position;

    public void Init(IPlayer player, EnemyStats stats)
    {
        Player = player ?? throw new Exception($"Player is null");

        _storedDamage = 0;

        CurrentStats = stats;

        _currentHealth = CurrentStats.MaxHealth;
        
        _health.text = $"{_currentHealth:f1}";
        
        IsAlive = true;
    }

    protected override void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentEffects = new List<IEffect<IAttacker>>();
        
        StateMachine = new StateMachine();
        IdleState = new IdleState(this, Animator);
        _dieState = new DieState(this, Animator);
        RunState = new RunState(this, Animator);
    }

    protected override void Update()
    {
        StateMachine.Update();
    }

    protected override void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    protected virtual void OnEnable()
    {
        InitializeStateMachine();
    }

    private void Start()
    {
        Collider.center = _center;
        Collider.size = _colliderSize;
        DamageZone.SetSettings(_colliderSize, _center);
    }

    public void ResetCharacteristics() { }

    public override void Release() => CanBeReleased?.Invoke(this);
    
    public virtual void Upgrade(EnemyStats stats)
    {
        CurrentStats = stats;
    }

    public void TakeDamage(float damage)
    {
        TookDamage?.Invoke(this);
        
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);
        
        damage = damage.AddPercentToNumber(CurrentStats.IncomingDamageMultiplier);

        _storedDamage += damage;
        
        _health.text = $"{CurrentHealth:f1}";
        
        if (!(CurrentHealth <= 0) || !IsAlive)
            return;
        
        IsAlive = false;
            
        Die();
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
        var tempSpeed = isSlowing ? Mover.Speed.SubtractPercentFromNumber(speedValue) : Mover.Speed.AddPercentToNumber(speedValue);

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
        MoveTowards(Player.CurrentPosition);
    }

    public override void HandleAttack() { }
    
    public void SetColliderCenter(Vector3 offsetAfterDeath, bool isResetting)
    {
        if (isResetting)
        {
            Collider.center -= offsetAfterDeath;
            DamageZone.gameObject.transform.position -= offsetAfterDeath;
        }
        else
        {
            Collider.center += offsetAfterDeath;
            DamageZone.gameObject.transform.position += offsetAfterDeath;
        }
    }
    
    protected override void InitializeStateMachine()
    {
        DefineAtTransition(IdleState, RunState, new FuncPredicate(() => IsAlive));
        
        DefineAnyTransition(_dieState, new FuncPredicate(() => CurrentHealth <= 0));

        StateMachine.SetState(IdleState);
    }
    
    protected virtual void OnAttackerDetected(IAttacker attacker, IWeapon weapon)
    {
        if (attacker is IPlayer player)
        {
            player.TakeDamage(weapon.Damage);
        }
    }
    
    protected void MoveTowards(Vector3 position)
    {
        Mover.MoveToPosition(position, CurrentStats.MovementSpeed);
        
        Vector3 direction = (position - transform.position).normalized;
        
        Rotator.Rotate(direction);
    }
    
    protected virtual void Die()
    {
        NoHealthLeft?.Invoke(this);
        
        _movementEffectCount = 0;

        _health.text = $"{CurrentHealth}";
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireCube(_center, _colliderSize);
    }
}