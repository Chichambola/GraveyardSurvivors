using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEditor;
using UnityEngine;

public class Chest : ChanceInteractable<Chest>, IPoolable<Chest>
{
    public const string IsOpened = nameof(IsOpened); 
    
    [SerializeField] private int _countdownTime = 3;
    [SerializeField] private Animator _animator;
    
    public event Action<Chest> CanBeReleased;
    public override event Action<Chest> WasChosen;
    
    private Coroutine _coroutine;
    private float _initialCost;

    private void Start()
    {
        if (Cost <= 0)
            throw new Exception(nameof(Cost));

        _initialCost = Cost;
    }

    public override void ProcessInteraction()
    {
        if (!IsAvailable) 
            return;
        
        IsAvailable = false;
        
        _animator.SetBool(IsOpened, true);
        
        ChangeOutlineVisibility(false);
        
        WasChosen?.Invoke(this);
    }

    public void Release()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(ReleaseCountdown());
    }

    private IEnumerator ReleaseCountdown()
    {
        float timePassed = 0;
        
        while (timePassed < _countdownTime)
        {
            timePassed += Time.deltaTime;
            
            yield return null;
        }

        ResetCharacteristics();
        
        CanBeReleased?.Invoke(this);
    }

    public void ResetCharacteristics()
    {
        SetCost(_initialCost);
        IsAvailable = true;
    }
}
