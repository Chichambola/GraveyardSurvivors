using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltar : Interactable, IPoolable<BloodAltar>
{
    [SerializeField] private List<float> _damagePercent;
    
    private Dictionary<int,float> _damagePercentDict;
    private int _maxInteractionsAmount;
    private int _currentInteractionsAmount = 0;
    
    public event Action<BloodAltar> CanBeReleased;
    public event Action<BloodAltar> WasChosen;

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

    public void ResetCharacteristics()
    {
        _maxInteractionsAmount = 0;
        _currentInteractionsAmount = 0;
        IsAvailable = true;
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public override void ProcessInteraction()
    {
        if(IsAvailable == false)
            return;
        
        WasChosen?.Invoke(this);
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
}
