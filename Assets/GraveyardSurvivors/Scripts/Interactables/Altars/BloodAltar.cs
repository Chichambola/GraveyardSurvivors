using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltar : Interactable
{
    [SerializeField] private CooldownHandler _cooldownHandler;
    [SerializeField] private List<float> _damagePercent;
    
    private Dictionary<int,float> _damagePercentDict;
    private int _maxInteractionsAmount;
    private int _currentInteractionsAmount;

    private void OnEnable()
    {
        FillDictionary();
    }
    
    private void FillDictionary()
    {
        _damagePercentDict = new Dictionary<int,float>();
        _maxInteractionsAmount = 0;
        
        foreach (var damagePercent in _damagePercent)
        {
            _damagePercentDict.Add(_maxInteractionsAmount, damagePercent);
            
            _maxInteractionsAmount++;
        }
        
        if (_damagePercentDict.Count == 0)
            throw new Exception();
        
        SetValue(_damagePercentDict[_currentInteractionsAmount]);
    }

    public override void ResetCharacteristics()
    {
        _maxInteractionsAmount = 0;
        _currentInteractionsAmount = 0;
        IsAvailable = true;
    }
    
    public float GetDamagePercent()
    {
        return _damagePercentDict[_currentInteractionsAmount];
    }

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount ++;

        if(_currentInteractionsAmount >= _maxInteractionsAmount)
            IsAvailable = false;
        else
            SetValue(_damagePercentDict[_currentInteractionsAmount]);
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
