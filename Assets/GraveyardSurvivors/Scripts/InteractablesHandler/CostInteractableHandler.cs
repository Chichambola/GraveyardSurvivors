using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CostInteractableHandler : InteractableHandler
{
    [SerializeField] protected float Cost;
    [SerializeField] protected float IncreasePercent = 40f;

    private float _initialCost;

    private void OnEnable()
    {
        _initialCost = Cost;
    }

    public void IncreaseCost()
    {
        Cost = Mathf.Round(UserUtils.AddPercentToNumber(Cost, IncreasePercent));
    }

    public float IncreaseCost(float cost)
    {
        cost = Mathf.Round(UserUtils.AddPercentToNumber(cost, IncreasePercent));
        
        return cost;
    }
    
    private void ResetCost()
    {
        Cost = _initialCost;

        InteractableSpawner.SetValueForObjects(Cost);
    }
}
