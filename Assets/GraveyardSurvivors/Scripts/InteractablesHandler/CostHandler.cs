using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CostHandler : MonoBehaviour
{
    [SerializeField] private float _cost;
    [SerializeField] private float _increasePercent = 40f;
    [SerializeField] private float _costThreshold = 1000;

    public float Cost => _cost;

    public void IncreaseCost() => _cost = Mathf.Round(_cost.GetClampedValue(_increasePercent, _costThreshold));

    public float IncreaseCost(float cost)
    {
        cost = Mathf.Round(cost.GetClampedValue(_increasePercent, _costThreshold));
        
        return cost;
    }
}
