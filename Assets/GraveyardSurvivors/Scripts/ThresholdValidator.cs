using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class ThresholdValidator : MonoBehaviour
{
    public event Action ThresholdReached;
    
    private Sequence _checkValueSequence;
    private CancellationTokenSource _cts;
    private IValueOwner _valueOwner;
    private readonly float _timeCheck = 0.02f;
    private readonly int _amountOfCycles = -1;
    private float _disableThreshold;
    
    public void Execute(IValueOwner valueOwner, float disableThreshold)
    {
        StopValidating();
        
        _valueOwner = valueOwner;
        _disableThreshold = disableThreshold;
        
        CreateToken();
        
        CreateThresholdSequence().Forget();
    }
    
    private async UniTaskVoid CreateThresholdSequence()
    {
        _checkValueSequence = Sequence.Create(cycles: _amountOfCycles).Group(Tween.Delay(_timeCheck, OnThresholdReached));
        
        await _checkValueSequence.ToYieldInstruction().WithCancellation(_cts.Token);
    }
    
    private void OnThresholdReached()
    {
        if (!(_valueOwner.Value < _disableThreshold))
            return;
        
        ThresholdReached?.Invoke();
    }

    private void CreateToken()
    {
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
    }

    public void StopValidating()
    {
        if (!_checkValueSequence.isAlive)
            return;
        
        _checkValueSequence.Stop();
        _cts.Cancel();
        _valueOwner = null;
    }
}
