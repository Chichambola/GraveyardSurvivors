using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Player : MonoBehaviour, IBuffable, IAttacker, IPlayerStats
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
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        _collisionDetector.ItemDetected += AddBuff;
        _health.ValueChanged += OnHealthValueChanged;
        
        if (_baseStats == null)
            throw new Exception();

        CurrentStats = _baseStats.GetStats();

        SetStats();
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
}