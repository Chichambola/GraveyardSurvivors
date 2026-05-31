using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CostInteractableHandler : InteractableHandler
{
    [SerializeField] protected float Cost;
    [SerializeField] protected float IncreasePercent = 40f;
    [SerializeField] private float _costThreshold = 1000;

    private float _initialCost;

    private void OnEnable()
    {
        _initialCost = Cost;
    }

    protected void IncreaseCost()
    {
       Cost = Mathf.Round(Cost.GetClampedValue(IncreasePercent, _costThreshold));
    }

    protected float IncreaseCost(float cost)
    {
        cost = Mathf.Round(cost.GetClampedValue(IncreasePercent, _costThreshold));
        
        return cost;
    }
    
    private void ResetCost()
    {
        Cost = _initialCost;

        InteractableSpawner.SetValueForObjects(Cost);
    }

    public void SetValueForObjects()
    {
        InteractableSpawner.SetValueForObjects(Cost);
    }
}
