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
    [SerializeField] private float _shrinkRate = 0.2f;
    
    public event Action<LightAltar> WaitTimeEnded;
    
    private int _currentInteractionsAmount;
    private int _defaultValue = 0;
    private float _radius;
    private Coroutine _coroutine;

    private void Start()
    {
        float threshold = _maxInteractionsAmount * _radiusMultiplier;
        
        _light.SetBaseSettings(threshold, _shrinkRate);
        
        _light.ChangeState(false);
    }

    public override void ProcessInteraction()
    {
        base.ProcessInteraction();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(WaitCoroutine());
    }

    public override void ResetCharacteristics()
    {
        _currentInteractionsAmount = _defaultValue;
        IsAvailable = true;
    }

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount++;

        if (_currentInteractionsAmount == _maxInteractionsAmount)
        {
            IsAvailable = false;
        }
    }

    public void StopShrinking() => _light.SetRate(_defaultValue);

    public void StartShrinking()
    {
        _light.SetGainingEnergyState(false);

        _light.SetRate(_shrinkRate);

        _light.StartRadiusRoutine(_defaultValue);
    }

    public void StartExpanding()
    {
        _radius += _radiusMultiplier;

        if (_radius <= 0)
            throw new Exception("Radius can not be lesser than 0");
            
        _light.SetGainingEnergyState(true);

        _light.StartRadiusRoutine(_radius);
    }
    
    private IEnumerator WaitCoroutine()
    {
        var wait = new WaitForSecondsRealtime(_timeBeforeShrinking);

        while (enabled)
        {
            yield return wait;
            
            WaitTimeEnded?.Invoke(this);
        }
    }
}
