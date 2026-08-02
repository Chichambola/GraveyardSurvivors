using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float _radiusMultiplier = 2f;
    [SerializeField] private float _cooldown = 1.5f;
    [SerializeField] private float _healAmount = 3f;
    [SerializeField] private PlayerDetector _playerDetector;
    [FormerlySerializedAs("_radiusEffectHandler")] [SerializeField] private RadiusEffectScaler radiusEffectScaler;
    
    private int _currentInteractionsAmount;
    private float _radius;
    private IPlayer _player;
    private CancellationTokenSource _cts;

    private void OnEnable()
    {
        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _playerDetector.PlayerLeft -= OnPlayerLeft;
    }

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount++;
        
        _radius += _radiusMultiplier;
        
        if (_currentInteractionsAmount == _maxInteractionsAmount)
        {
            IsAvailable = false;
        }
    }
    
    private void OnPlayerDetected(IPlayer player)
    {
        _player = player;
        
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);

        HealTask().Forget();
    }

    private void OnPlayerLeft()
    {
        _cts?.Cancel();
        
        _player = null;
    }
    
    private async UniTask HealTask()
    {
        while (!_cts.IsCancellationRequested)
        {
            _player.Heal(_healAmount);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_cooldown), cancellationToken: _cts.Token);
        }
    }
}
