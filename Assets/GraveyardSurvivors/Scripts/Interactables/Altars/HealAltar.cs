using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HealAltar : Interactable
{
    [SerializeField] private int _maxInteractionsAmount = 3;
    [Header("Lantern")]
    [SerializeField] private float _radiusMultiplier = 2f;
    
    private int _currentInteractionsAmount;
    private int _defaultValue = 0;
    private float _radius;
    private Coroutine _coroutine;
    
    public override void ResetCharacteristics()
    {
        _currentInteractionsAmount = _defaultValue;
        IsAvailable = true;
    }

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount++;
        
        _radius += _radiusMultiplier;
        
        if (_currentInteractionsAmount == _maxInteractionsAmount)
        {
            IsAvailable = false;
        }
    }
}
