using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>
{
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

    public EnemyStats CurrentStats { get; private set; }
    public Rigidbody Rigidbody => _rigidbody;
    public float Damage => _weapon.Info.Damage;

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
        
        _health.text = $"{_initialHealth}";
    }

    public void ResetCharacteristics()
    {
        CurrentStats.Health = _initialHealth;
    }

    public void Release() => CanBeReleased?.Invoke(this);

    public void TakeDamage(float damage)
    {
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);

        CurrentStats.Health -= damage;

        _health.text = $"{CurrentStats.Health}";
        
        if (CurrentStats.Health <= 0)
        {
            Die();
        }
    }

    public void ApplyEffect(IEffect<IAttacker> effect)
    {
        effect.EffectCompleted += RemoveEffect;
        _currentEffects.Add(effect);
        effect.Apply(this);
    }

    private void RemoveEffect(IEffect<IAttacker> effect)
    {
        effect.EffectCompleted -= RemoveEffect;
        _currentEffects.Remove(effect);
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
        CurrentStats.Health = 0;
        
        _collider.enabled = false;

        _health.text = $"{CurrentStats.Health}";
        
        RemoveAllEffects();
    }
    
    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        var attackState = new EnemyAttackState(this, Animator);
        var dieState = new DieState(this, Animator);

        DefineAtTransition(idleState, runState, new FuncPredicate(() => _player != null));
        DefineAtTransition(runState, attackState, new FuncPredicate(() => _playerDetector.IsPlayerNear));
        DefineAtTransition(attackState, runState,
            new FuncPredicate(() => !_playerDetector.IsPlayerNear && !_weapon.IsAttacking));
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentStats.Health <= 0));

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
}