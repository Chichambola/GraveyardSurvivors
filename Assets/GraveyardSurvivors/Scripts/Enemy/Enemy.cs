using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : CharacterBase, IAttacker, IPoolable<Enemy>
{
    [SerializeField] private EnemyInfo _info;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private WeaponSkeletonArm _weapon;
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private Defender _defender;

    public event Action<Enemy> CanBeReleased;

    private Player _player;
    private CapsuleCollider _collider;
    private List<IEffect<IAttacker>> _currentEffects;

    public EnemyStats CurrentStats { get; private set; }
    public bool IsAttacking { get; private set; }
    public float Damage => _weapon.Info.Damage;

    public void Init(Player player)
    {
        _player = player;
    }

    protected override void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _currentEffects = new List<IEffect<IAttacker>>();

        CurrentStats = _info.GetStats();

        InitializeStateMachine();
    }

    private void OnEnable()
    {
        _weapon.FinishedAttacking += OnWeaponFinishedAttacking;
    }

    private void OnDisable()
    {
        _weapon.FinishedAttacking -= OnWeaponFinishedAttacking;
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
        _collider.enabled = true;
    }

    public void Release() => CanBeReleased?.Invoke(this);

    public void TakeDamage(float damage)
    {
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);

        CurrentStats.Health -= damage;

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
        Mover.MoveTowardsTarget(_player.transform, CurrentStats.MovementSpeed);

        Vector3 direction = UserUtils.GetDirection(_player.transform.position, transform.position);

        Rotator.Rotate(direction);
    }

    public override void HandleAttack()
    {
        IsAttacking = true;

        _weapon.Attack(_attackCooldown);
    }

    private void OnWeaponFinishedAttacking(Weapon weapon)
    {
        IsAttacking = false;

        weapon.StopAttacking();
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
            new FuncPredicate(() => !_playerDetector.IsPlayerNear && !IsAttacking));
        DefineAnyTransition(dieState, new FuncPredicate(() => CurrentStats.Health <= 0));

        StateMachine.SetState(idleState);
    }

    private void Die()
    {
        _collider.enabled = false;

        RemoveAllEffects();
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