using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceAltar : CooldownInteractable, IChanceInteractable
{
    [SerializeField] private RarityLevelHandler _levelHandler;
    
    public override event Action<Interactable> WasChosen;
    
    private float _currentCost;
    private Coroutine _coroutine;
    
    public List<RarityLevel> Weights => _levelHandler.Weights;

    public float CurrentCost => _currentCost;

    public override void ProcessInteraction()
    {
        if (IsAvailable == false || CurrentInteractionsAmount == MaxInteractionsAmount)
            return;
        
        SetVisibility(false);
        
        WasChosen?.Invoke(this);
    }
    
    public override void ResetCharacteristics()
    {
        IsAvailable = true;
        CurrentInteractionsAmount = 0;
    }

    public override void SetValue(float value)
    {
        _currentCost = value;
        
        base.SetValue(value);
    }

    public void IncreaseInteractionsAmount()
    {
        CurrentInteractionsAmount++;
    }
}
