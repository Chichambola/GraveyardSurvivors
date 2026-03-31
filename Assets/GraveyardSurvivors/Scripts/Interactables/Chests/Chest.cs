using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEditor;
using UnityEngine;

public class Chest : Interactable, IChanceInteractable
{
    public const string IsOpened = nameof(IsOpened);
    
    [Header("Animator values")]
    [SerializeField] private int _countdownTime = 3;
    [SerializeField] private Animator _animator;
    [Header("Weights")]
    [SerializeField] protected int CommonChanceWeight;
    [SerializeField] protected int RareChanceWeight;
    [SerializeField] protected int LegendaryChanceWeight;
    
    public override event Action<Interactable> CanBeReleased;

    private Coroutine _coroutine;

    public int CommonChance => CommonChanceWeight;
    public int RareChance => RareChanceWeight;
    public int LegendaryChance => LegendaryChanceWeight;

    public void Open()
    {
        IsAvailable = false;
        
        _animator.SetBool(IsOpened, true);
        
        ChangeOutlineVisibility(false);
    }

    public override void Release()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(ReleaseCountdown());
    }

    public override void ResetCharacteristics()
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
