using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _initialValue = 50;

    private float _currentMoneyAmount;

    public float CurrentMoneyAmount => _currentMoneyAmount;
    
    private void OnEnable()
    {
        _currentMoneyAmount = _initialValue;
    }

    private void Start()
    {
        UpdateAmount(_currentMoneyAmount);
    }

    public void ReduceMoneyAmount(float amount)
    {
        _currentMoneyAmount -= amount;

        UpdateAmount(_currentMoneyAmount);
    }

    public void ReceiveMoney(float value)
    {
        _currentMoneyAmount += value;
        
        UpdateAmount(_currentMoneyAmount);
    }
    
    private void UpdateAmount(float value)
    {
        _text.text = $"Money: {value}";
    }
}
