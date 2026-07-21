using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>
{
    [SerializeField] private DamageZone _damageZone;
    [Header("Services")]
    [SerializeField] protected PlayerDetector PlayerDetector;
    [SerializeField] protected InterfaceReference<IWeapon, MonoBehaviour> Weapon;
    [SerializeField] private TextMeshProUGUI _health;
    [SerializeField] private Defender _defender;
    
    public event Action<Enemy> CanBeReleased;
    public event Action<Enemy> NoHealthLeft;
    public event Action<Enemy> TookDamage; 
    
    protected IPlayer Player;
    private Coroutine _attackRoutine;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private List<IEffect<IAttacker>> _currentEffects;
    private float _currentHealth;
    private float _storedDamage;
    private int _movementEffectCount;

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

    protected virtual void OnEnable()
    {
        InitializeStateMachine();

        Weapon.Value.AttackerDetected += OnAttackerDetected;
    }

    protected virtual void OnDisable()
    {
        Weapon.Value.AttackerDetected -= OnAttackerDetected;
    }

    public void ResetCharacteristics()
    {

    }

    public override void Release() => CanBeReleased?.Invoke(this);
    
    public void Upgrade(EnemyStats stats)
    {
        CurrentStats = stats;
        Weapon.Value.Upgrade();
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
        MoveTowards(Player.CurrentPosition);
    }

    public override void HandleAttack()
    {
        Weapon.Value.Attack();
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
            new FuncPredicate(() => !PlayerDetector.IsPlayerNear && !Weapon.Value.IsAttacking));
        
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentHealth <= 0));
        DefineAnyTransition(attackState, new FuncPredicate(() => CurrentHealth >= 0 && PlayerDetector.IsPlayerNear));

        StateMachine.SetState(idleState);
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
    
    private void Die()
    {
        NoHealthLeft?.Invoke(this);
        
        _movementEffectCount = 0;

        _health.text = $"{CurrentHealth}";

        Weapon.Value.StopAttacking();
        
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
}