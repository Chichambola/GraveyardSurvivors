using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceAltar : CooldownInteractable, IChanceInteractable
{
    [Header("Weights")]
    [SerializeField] private RarityLevel _common;
    [SerializeField] private RarityLevel _rare;
    [SerializeField] private RarityLevel _legendary;
    [SerializeField] private RarityLevel _none;
    
    public override event Action<Interactable> WasChosen;
    
    private float _currentCost;
    private Coroutine _coroutine;
    private List<RarityLevel> _weights;
    
    public float CurrentCost => _currentCost;
    public List<RarityLevel> Weights => _weights;

    private void Awake()
    {
        _weights = new() { _none, _common, _rare, _legendary};
    }

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
