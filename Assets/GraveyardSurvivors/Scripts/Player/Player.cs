using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Player : MonoBehaviour, IBuffable, IAttacker
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

    public event Action InteractionButtonPressed;
    public event Action<CharacterStats> StatsChanged;
    
    private readonly List<IBuff> _buffs = new ();
    
    public CharacterStats CurrentStats { get; private set; }
    
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
    }

    private void OnDisable()
    {
        _collisionDetector.ItemDetected -= AddBuff;
    }

    private void Start()
    {
        StatsChanged?.Invoke(CurrentStats);
    }

    private void FixedUpdate()
    {
        _mover.Move(_inputReader.MovementDirection.normalized, CurrentStats.MovementSpeed);
            
        if (_inputReader.MovementDirection != Vector3.zero)
        {
            _rotator.Rotate(_inputReader.MovementDirection.normalized);
        }

        if (_inputReader.IsInteractionButtonPressed)
        {
            InteractionButtonPressed?.Invoke();
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

        Debug.Log(_wallet.CurrentMoneyAmount);
    }
    
    public bool HasEnoughHealth(float value)
    {
        throw new NotImplementedException();
    }
    
    public void TakeDamage(float damage)
    {
        var stats = CurrentStats;
        
        stats.Health -= damage;
        
        CurrentStats = stats;
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
        CurrentStats = _baseStats.Stats;

        foreach (var buff in _buffs)
        {
            CurrentStats = buff.ApplyBuff(CurrentStats);
        }
        
        StatsChanged?.Invoke(CurrentStats);
    }
}