using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltar : Interactable, IPoolable<BloodAltar>
{
    [SerializeField] private List<int> _damagePercent;
    
    private Dictionary<int,int> _damagePercentDict;
    private int _maxInteractionsAmount;
    private int _currentInteractionsAmount;
    
    public event Action<BloodAltar> CanBeReleased;
    public event Action<BloodAltar> WasChosen;

    private void Start()
    {
        FillDictionary();
    }

    private void FillDictionary()
    {
        _damagePercentDict = new Dictionary<int,int>();
        _maxInteractionsAmount = 0;
        
        foreach (var damagePercent in _damagePercent)
        {
            _damagePercentDict.Add(_maxInteractionsAmount, damagePercent);
            
            _maxInteractionsAmount++;
        }
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
    
    public int GetDamagePercent()
    {
        return _damagePercentDict[_currentInteractionsAmount];
    }

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount ++;

        if(_currentInteractionsAmount >= _maxInteractionsAmount)
            IsAvailable = false;
    }
}
