using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CooldownInteractable : Interactable
{
    [SerializeField] protected int MaxInteractionsAmount = 2;
    [SerializeField] private float _countdownTime = 1.5f;
    
    private Coroutine _cooldownRoutine;
    protected int CurrentInteractionsAmount;
    
    public void StartCountdown()
    {
        if(_cooldownRoutine != null)
            StopCoroutine(_cooldownRoutine);    
        
        SetVisibility(false);

        StartCoroutine(CooldownRoutine());
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

        if (CurrentInteractionsAmount != MaxInteractionsAmount)
            IsAvailable = true;
        
        yield return null;
    }
}
