using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HealAltar : Interactable
{
    [Header("Altar specific fields")]
    [SerializeField] private int _maxInteractionsAmount = 3;
    [SerializeField] private List<float> _radiusMultipliers;
    [SerializeField] private float _cooldown = 1.5f;
    [SerializeField] private float _healAmount = 3f;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private RadiusEffectScaler _radiusEffectScaler;
    [SerializeField] private WaterAnimationHandler _animationHandler;

    public event Action DetectedPlayer;
    public event Action LostPlayer;
    
    private const float DefaultRadiusMultiplier = 2f;
    private float _radius;
    private float _initialRadius;
    private int _currentInteractionsAmount;
    private ILightCarrier _lightCarrier;
    private CancellationTokenSource _cts;

    private void OnValidate()
    {
        if (_maxInteractionsAmount <= 0)
            _maxInteractionsAmount = 0;
        
        DetermineMultipliersCount();
    }

    private void OnEnable()
    {
        _radiusEffectScaler.SetActive(false);
        
        _animationHandler.Init(_radiusMultipliers.Count);

        if (_radiusMultipliers.Count == 0) 
            throw new Exception("You need more multipliers!");
        
        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _playerDetector.PlayerLeft -= OnPlayerLeft;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void IncreaseInteractionAmount()
    {
        if (!_radiusEffectScaler.IsActive)
            _radiusEffectScaler.SetActive(true);
        
        _currentInteractionsAmount++;

        float multiplier = _radiusMultipliers.First();
        
        _radius += multiplier;

        _radiusMultipliers.Remove(multiplier);

        _radiusEffectScaler.SetInitialRadius(_radius);
        
        _radiusEffectScaler.ChangeRadius(_radius, _cooldown);
        
        _animationHandler.StartAnimation(_currentInteractionsAmount);
        
        if (_currentInteractionsAmount == _maxInteractionsAmount)
        {
            IsAvailable = false;
        }
    }
    
    private void OnPlayerDetected(IPlayer player)
    {
        if (player is not ILightCarrier carrier)
            return;

        DetectedPlayer?.Invoke();
        
        _lightCarrier = carrier;

        _lightCarrier.PauseLight();

        _cts = new CancellationTokenSource();

        HealTask(_cts.Token).Forget();
    }

    private void OnPlayerLeft()
    {
        _cts?.Cancel();
        
        LostPlayer?.Invoke();

        _lightCarrier = null;
    }
    
    private async UniTask HealTask(CancellationToken token)
    {
        while (!_cts.IsCancellationRequested)
        {
            _lightCarrier.Heal(_healAmount);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_cooldown), cancellationToken: token);
        }
    }
    
    private void DetermineMultipliersCount()
    {
        int targetNumber;

        if (_radiusMultipliers.Count != _maxInteractionsAmount)
        {
            targetNumber = _maxInteractionsAmount - _radiusMultipliers.Count;
            
            for (int i = 0; i < targetNumber; i++)
            {
                _radiusMultipliers.Add(DefaultRadiusMultiplier);
            }            
        }

        if (_radiusMultipliers.Count > _maxInteractionsAmount)
        {
            targetNumber = _radiusMultipliers.Count - _maxInteractionsAmount;
            
            for (int i = 0; i < targetNumber; i++)
            {
                _radiusMultipliers.Remove(_radiusMultipliers.Last());
            }  
        }
    }
}