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
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CollisionDetector _collisionDetector;
    [SerializeField] private InteractorHandler _InteractorHandler;
    [SerializeField] private Attacker _attacker;
    
    [Header("Stats")]
    [SerializeField] private PlayerInfo _baseStats;
    [SerializeField] private StatsViewer _statsViewer;

    [Header("Services")] 
    [SerializeField] private HealthRegenerator _regenerator;
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private Wallet _wallet;
    
    public event Action InteractionButtonPressed;
    public event Action<CharacterStats> StatsChanged;
    
    public CharacterStats CurrentStats { get; private set; }
    
    public float MoneyAmount => _wallet.CurrentMoneyAmount;
    public float CurrentHealth => CurrentStats.Health;
    public float MaxHealth { get; private set; }

    protected override void Awake()
    {
        _inputReader = GetComponent<InputReader>();

        InitializeStateMachine();
    }

    private void OnEnable()
    {
        if (_baseStats == null)
            throw new Exception();

        CurrentStats = _baseStats.GetStats();
        MaxHealth = CurrentStats.Health;
        
        _collisionDetector.ItemDetected += AddBuff;
        _regenerator.HealthRegenerated += OnHeal;
        
        _attacker.StartAttacking(CurrentStats.AttackSpeed, CurrentStats.AttackRadius);
    }

    private void OnDisable()
    {
        _collisionDetector.ItemDetected -= AddBuff;
        _regenerator.HealthRegenerated -= OnHeal;
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
    
    public void ReduceMoneyAmount(float amount)
    {
        _wallet.ReduceMoneyAmount(amount);
    }

    public void ReceiveMoney(float value)
    {
        _wallet.ReceiveMoney(value);
    }
    
    public void TakeDamage(float damage)
    {
        damage = DetermineDamageAmount(damage);

        CurrentStats.Health -= damage;
        
        StatsChanged?.Invoke(CurrentStats);
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
    
    private void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        
        DefineAtTransition(idleState, runState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude > 0));
        DefineAtTransition(runState, idleState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude <= 0));
        
        StateMachine.SetState(idleState);
    }
    
    private float DetermineDamageAmount(float damage)
    {
        if (_evader.CanEvade(CurrentStats.EvasionChance, CurrentStats.Luck))
        {
            Debug.Log("Evaded");

            return damage;
        }
        
        if (_defender.CanBlock(CurrentStats.BlockChance, CurrentStats.Luck))
        {
            Debug.Log("Blocked");
            
            damage = _defender.GetBlockedDamage(damage);
        }
        
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);
        
        return damage;
    }
    
    private void OnHeal(float value)
    {
        CurrentStats.Health += value;

        if (CurrentStats.Health >= MaxHealth)
        {
            CurrentStats.Health = MaxHealth;
        }
        
        StatsChanged?.Invoke(CurrentStats);
    }
}