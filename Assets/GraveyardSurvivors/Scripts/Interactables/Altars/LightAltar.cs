using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LightAltar : Interactable
{
    [SerializeField] private int _maxInteractionsAmount = 3;
    [SerializeField] private float _timeBeforeShrinking = 5f;
    [Header("Lantern")]
    [SerializeField] private LanternLight _light;
    [SerializeField] private float _radiusMultiplier = 2f;
    
    private int _currentInteractionsAmount;
    private int _defaultValue = 0;
    private float _radius;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _light.GainedEnergy += StartWaiting;
    }

    private void OnDisable()
    {
        _light.GainedEnergy -= StartWaiting;
    }

    public override void ResetCharacteristics()
    {
        _currentInteractionsAmount = _defaultValue;
        IsAvailable = true;
    }

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount++;
        
        _radius += _radiusMultiplier;
        
        _light.SetRate(_defaultValue);
        
        _light.StartRadiusRoutine(_radius);
        
        if (_currentInteractionsAmount == _maxInteractionsAmount)
        {
            IsAvailable = false;
        }
    }

    private void StartWaiting()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(WaitCoroutine());
    }
    
    private IEnumerator WaitCoroutine()
    {
        var wait = new WaitForSecondsRealtime(_timeBeforeShrinking);

        while (enabled)
        {
            yield return wait;
            
            _light.ResetRate();
            
            _light.StartRadiusRoutine();
        }
    }
}
