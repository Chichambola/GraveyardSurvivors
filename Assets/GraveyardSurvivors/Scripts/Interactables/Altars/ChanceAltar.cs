using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChanceAltar : ChanceInteractable<ChanceAltar>, IPoolable<ChanceAltar>
{
    [SerializeField] private int _maxInteractionsAmount = 2;
    [SerializeField] private float _countdownTime = 1.5f;

    private float _currentCost;
    
    public float CurrentCost => _currentCost;
    
    public override event Action<ChanceAltar> WasChosen;
    public event Action<ChanceAltar> CanBeReleased;
    
    private int _currentInteractionsAmount;
    private Coroutine _coroutine;
    
    public override void ProcessInteraction()
    {
        if (IsAvailable == false || _currentInteractionsAmount == _maxInteractionsAmount)
            return;
        
        WasChosen?.Invoke(this);
    }

    public void StartCountdown()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);    
        
        ChangeOutlineVisibility(false);

        StartCoroutine(CooldownRoutine());
    }
    
    public void ResetCharacteristics()
    {
        IsAvailable = true;
        _currentInteractionsAmount = 0;
    }

    public void SetCost(float value)
    {
        _currentCost = value;
        
        ValueViewer.SetValue(_currentCost);
    }
    
    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void IncreaseInteractionsAmount()
    {
        _currentInteractionsAmount++;
    }
    
    private IEnumerator CooldownRoutine()
    {
        IsAvailable = false;
        
        float timePassed = 0;
        
        while (timePassed < _countdownTime)
        {
            timePassed += Time.deltaTime;
            
            yield return null;
        }

        if (_currentInteractionsAmount != _maxInteractionsAmount)
            IsAvailable = true;

        yield return null;
    }
}
