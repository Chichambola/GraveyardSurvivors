using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CostHandler : MonoBehaviour
{
    [SerializeField] private float _value;
    [SerializeField] private float _increasePercent = 40f;
    [SerializeField] private float _costThreshold = 1000;

    public float Value => _value;

    public void Increase() => _value = Mathf.Round(_value.GetClampedValue(_increasePercent, _costThreshold));

    public float Increase(float cost)
    {
        cost = Mathf.Round(cost.GetClampedValue(_increasePercent, _costThreshold));
        
        return cost;
    }
}
