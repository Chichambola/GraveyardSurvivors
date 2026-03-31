using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceAltar : CooldownInteractable, IChanceInteractable
{
    [Header("Weights")]
    [SerializeField] protected int CommonChanceWeight;
    [SerializeField] protected int RareChanceWeight;
    [SerializeField] protected int LegendaryChanceWeight;
    [SerializeField] private int _noneChance = 40;
    
    public override event Action<Interactable> WasChosen;
    
    private float _currentCost;
    private Coroutine _coroutine;
    
    public float CurrentCost => _currentCost;
    public int NoneChance => _noneChance;
    public int CommonChance => CommonChanceWeight;
    public int RareChance => RareChanceWeight;
    public int LegendaryChance => LegendaryChanceWeight;
    
    public override void ProcessInteraction()
    {
        if (IsAvailable == false || CurrentInteractionsAmount == MaxInteractionsAmount)
            return;
        
        HideValue();
        
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
