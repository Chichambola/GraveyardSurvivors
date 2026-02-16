using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChanceHandlerBase : InteractableHandler
{
    [SerializeField] protected float Cost;
    [SerializeField] protected int IncreasePercent;
    [SerializeField] protected RarityEvaluator RarityEvaluator;
    [SerializeField] protected ItemsHandler ItemsHandler;

    private float _initialValue;
    
    public float CurrentCost => Cost;
    
    private void Start()
    {
        _initialValue = Cost;
    }

    public virtual void ResetInitialValue()
    {
        Cost = _initialValue;
    }

    protected virtual void CalculateCost()
    {
        Cost *= (1 - (IncreasePercent / RarityEvaluator.HighestPercent));
    }
}
