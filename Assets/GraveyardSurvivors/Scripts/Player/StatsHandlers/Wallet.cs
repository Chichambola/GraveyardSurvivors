using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _initialValue = 50;

    private float _currentMoneyAmount;

    public float CurrentMoneyAmount => _currentMoneyAmount;
    
    private void OnEnable()
    {
        _currentMoneyAmount = _initialValue;
        
        UpdateValue();
    }
    
    private void Start()
    {
        UpdateValue();
    }

    public void ReduceMoneyAmount(float amount)
    {
        _currentMoneyAmount -= amount;
        
        UpdateValue();
    }

    public void ReceiveMoney(float value)
    {
        _currentMoneyAmount += value;
        
        UpdateValue();
    }

    private void UpdateValue()
    {
        _text.text = $"Money: {_currentMoneyAmount}";
    }
}
