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
    
    private void Start()
    {
        _currentMoneyAmount = _initialValue;
        
        UpdateValue();
    }

    public void ReduceMoney(float amount)
    {
        _currentMoneyAmount -= Mathf.Round(amount);
        
        UpdateValue();
    }

    public void ReceiveMoney(float value)
    {
        value *= _player.CurrentStats.GoldMultiplier;
        
        _currentMoneyAmount += value.RoundToFifths();
        
        UpdateValue();
    }

    private void UpdateValue()
    {
        _text.text = $"{_currentMoneyAmount}";
    }
}
