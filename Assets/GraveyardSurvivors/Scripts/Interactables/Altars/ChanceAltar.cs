using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceAltar : Interactable, IChanceInteractable
{
    [SerializeField] private CooldownHandler _cooldownHandler;
    [SerializeField] private RarityLevelHandler _levelHandler;
    [SerializeField] private int _maxInteractionsAmount = 3;
    
    public override event Action<Interactable> WasChosen;
    
    private float _currentCost;
    private int _currentInteractionsAmount;
    private Coroutine _coroutine;
    
    public List<RarityLevel> Weights => _levelHandler.Weights;

    public float CurrentCost => _currentCost;

    public override void ProcessInteraction()
    {
        if (IsAvailable == false || _currentInteractionsAmount == _maxInteractionsAmount)
            return;
        
        SetVisibility(false);
        
        WasChosen?.Invoke(this);
    }
    
    public override void ResetCharacteristics()
    {
        IsAvailable = true;
        _currentInteractionsAmount = 0;
    }

    public override void SetValue(float value)
    {
        _currentCost = value;
        
        base.SetValue(value);
    }

    public void IncreaseInteractionsAmount()
    {
        _currentInteractionsAmount++;
    }

    public void StartCountdown()
    {
        SetVisibility(false);

        IsAvailable = false;

        _cooldownHandler.TimePassed += OnTimePassed;
        
        _cooldownHandler.StartCountdown();
    }

    private void OnTimePassed()
    {
        if (_currentInteractionsAmount != _maxInteractionsAmount)
        {
            SetVisibility(true);
            
            IsAvailable = false;
        }
        
        _cooldownHandler.TimePassed -= OnTimePassed;
    }
}
