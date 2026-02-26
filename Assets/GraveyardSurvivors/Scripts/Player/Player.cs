using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Player : CharacterBase, IBuffable, IAttacker, IPlayerStats
{
    [Header("Base")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CollisionDetector _collisionDetector;
    [Header("Handlers")]
    [SerializeField] private InteractorHandler _InteractorHandler;
    [Header("Stats")]
    [SerializeField] private PlayerInfo _baseStats;
    [SerializeField] private StatsViewer _statsViewer;
    [Header("Stats handlers")]
    [SerializeField] private Health _health;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private Attacker _attacker;
    [SerializeField] private Weapon _weapon;
    
    public event Action InteractionButtonPressed;
    public event Action<CharacterStats> StatsChanged;
    
    public CharacterStats CurrentStats { get; private set; }
    public float MaxHealth => _health.MaxHealth;

    protected override void Awake()
    {
        _inputReader = GetComponent<InputReader>();

        InitializeStateMachine();
    }

    private void OnEnable()
    {
        _collisionDetector.ItemDetected += AddBuff;
        _health.ValueChanged += OnHealthValueChanged;
        
        if (_baseStats == null)
            throw new Exception();

        CurrentStats = _baseStats.GetStats();

        SetStats();
        
        _attacker.StartAttacking();
    }

    private void OnDisable()
    {
        _collisionDetector.ItemDetected -= AddBuff;
        _health.ValueChanged -= OnHealthValueChanged;
    }

    private void Start()
    {
        StatsChanged?.Invoke(CurrentStats);
    }

    protected override void Update()
    {
        StateMachine.Update();
        
        if (_inputReader.IsInteractionButtonPressed)
        {
            InteractionButtonPressed?.Invoke();
        }
    }

    protected override void FixedUpdate()
    {
        StateMachine.FixedUpdate();
        
        HandleMovement();
    }

    public override void HandleMovement()
    {
        Mover.Move(_inputReader.MovementDirection.normalized, CurrentStats.MovementSpeed);
        
        if (_inputReader.MovementDirection != Vector3.zero)
        {
            Rotator.Rotate(_inputReader.MovementDirection.normalized);
        }
    }
    
    public bool HasEnoughMoney(float amount)
    {
        return !(_wallet.CurrentMoneyAmount < amount);
    }
    
    public void ReduceMoneyAmount(float amount)
    {
        _wallet.ReduceMoneyAmount(amount);
    }

    public void ReceiveMoney(float value)
    {
        _wallet.ReceiveMoney(value);
    }
    
    public bool HasEnoughHealth(float value)
    {
        return !(_health.CurrentValue < value);
    }
    
    public void TakeDamage(float damage)
    {
        if (_evader.CanEvade(CurrentStats.Luck))
        {
            Debug.Log("Evaded");
            
            return;
        }
        
        if (_defender.CanBlock(CurrentStats.Luck))
        {
            Debug.Log("Blocked");
            
            damage = _defender.GetBlockedDamage(damage);
        }
        
        damage = _defender.GetDamageAmount(damage);
        
        _health.TakeDamage(damage);
    }
    
    public void AddBuff(IBuff buff)
    {
        CurrentStats = buff.ApplyBuff(CurrentStats);
        
        StatsChanged?.Invoke(CurrentStats);
    }

    public void RemoveBuff(IBuff buff)
    {
        CurrentStats = buff.RemoveBuff(CurrentStats);
        
        StatsChanged?.Invoke(CurrentStats);
    }
    
    private void OnHealthValueChanged(float value)
    {
        CurrentStats.Health = value;
        
        StatsChanged?.Invoke(CurrentStats);
    }
    
    private void SetStats()
    {
        _defender.SetInitialStats(CurrentStats);
        _health.SetInitialStats(CurrentStats);
        _evader.SetInitialStats(CurrentStats);
        _attacker.SetInitialStats(CurrentStats);
        _attacker.SetWeapon(_weapon);
    }
    
    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        
        DefineAtTransition(idleState, runState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude > 0));
        DefineAtTransition(runState, idleState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude <= 0));
        
        StateMachine.SetState(idleState);
    }
}