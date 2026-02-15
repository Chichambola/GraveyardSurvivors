using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Player : MonoBehaviour, IBuffable, IAttacker, IPlayer
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CollisionDetector _collisionDetector;
    [Header("Handlers")]
    [SerializeField] private AnimationHandler _controller;
    [SerializeField] private InteractorHandler _InteractorHandler;
    [Header("Movement")]
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [Header("Stats")]
    [SerializeField] private PlayerInfo _baseStats;
    [SerializeField] private StatsViewer _statsViewer;
    [SerializeField] private Wallet _wallet;

    private float _maxHealth;
    
    public event Action InteractionButtonPressed;
    public event Action<CharacterStats> StatsChanged;
    
    private readonly List<IBuff> _buffs = new ();
    
    public CharacterStats CurrentStats { get; private set; }
    public float MaxHealth => _maxHealth;
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        _collisionDetector.ItemDetected += AddBuff;
        
        if (_baseStats == null)
            throw new Exception();
        
        CurrentStats = _baseStats.Stats;
        _maxHealth = CurrentStats.Health;
    }

    private void OnDisable()
    {
        _collisionDetector.ItemDetected -= AddBuff;
    }

    private void Start()
    {
        StatsChanged?.Invoke(CurrentStats);
    }

    private void Update()
    {
        if (_inputReader.IsInteractionButtonPressed)
        {
            InteractionButtonPressed?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        _mover.Move(_inputReader.MovementDirection.normalized, CurrentStats.MovementSpeed);
            
        if (_inputReader.MovementDirection != Vector3.zero)
        {
            _rotator.Rotate(_inputReader.MovementDirection.normalized);
        }
            
        _controller.PlayMovementAnimation(_inputReader.MovementDirection.magnitude);
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
        return !(CurrentStats.Health < value);
    }
    
    public void TakeDamage(float damage)
    {
        var stats = CurrentStats;
        
        stats.Health -= damage;
        
        CurrentStats = stats;
        
        StatsChanged?.Invoke(CurrentStats);
    }
    
    public void AddBuff(IBuff buff)
    {
        _buffs.Add(buff);
        
        ApplyBuffs();
    }

    public void RemoveBuff(IBuff buff)
    {
        _buffs.Remove(buff);
        
        ApplyBuffs();
    }

    private void ApplyBuffs()
    {
        foreach (var buff in _buffs)
        {
            CurrentStats = buff.ApplyBuff(CurrentStats);
        }
        
        StatsChanged?.Invoke(CurrentStats);
    }
}