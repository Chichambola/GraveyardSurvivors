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

    public override void ProcessInteraction()
    {
        if (!IsAvailable) 
            return;
        
        WasChosen?.Invoke(this);
    }

    public void Open()
    {
        IsAvailable = false;
        
        _animator.SetBool(IsOpened, true);
        
        ChangeOutlineVisibility(false);
    }

    public void Release()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(ReleaseCountdown());
    }

    public void ResetCharacteristics()
    {
        IsAvailable = true;
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
}
