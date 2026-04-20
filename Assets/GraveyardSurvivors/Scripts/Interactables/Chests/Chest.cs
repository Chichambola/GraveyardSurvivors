using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEditor;
using UnityEngine;

public class Chest : Interactable, IChanceInteractable, IStateHandler
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
    private StateMachine _stateMachine;

    public int CommonChance => CommonChanceWeight;
    public int RareChance => RareChanceWeight;
    public int LegendaryChance => LegendaryChanceWeight;

    private void Awake()
    {
        _stateMachine = new StateMachine();
    }

    private void OnEnable()
    {
        var openingState = new OpeningState(this, _animator);
        var idleState = new IdleState(this, _animator);
        
        _stateMachine.AddAnyTransition(openingState, new FuncPredicate(() => IsAvailable == false));
        _stateMachine.AddAnyTransition(idleState, new FuncPredicate(() => IsAvailable));
        
        _stateMachine.SetState(idleState);
    }

    public void Open()
    {
        IsAvailable = false;
        
        _animator.SetBool(IsOpened, true);
        
        Release();
        
        SetVisibility(false);
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
