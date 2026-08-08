using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class ThresholdVerifier : MonoBehaviour
{
    [SerializeField] private RadiusEffectScaler _radius;
    
    public event Action ThresholdReached;
    
    private Sequence _checkValueSequence;
    private CancellationTokenSource _cts;
    private readonly float _timeCheck = 0.01f;
    private readonly int _amountOfCycles = -1;
    private float _disableThreshold;
    
    public void Execute(float disableThreshold)
    {
        _disableThreshold = disableThreshold;
        
        CreateToken();
        
        CreateThresholdSequence().Forget();
    }
    
    private async UniTaskVoid CreateThresholdSequence()
    {
        if (_disableThreshold <= 0)
            return;
        
        _checkValueSequence = Sequence.Create(cycles: _amountOfCycles).Group(Tween.Delay(_timeCheck, OnThresholdReached));
        
        await _checkValueSequence.ToYieldInstruction().WithCancellation(_cts.Token);
    }
    
    private void OnThresholdReached()
    {
        if (!(_radius.Value < _disableThreshold))
            return;
        
        ThresholdReached?.Invoke();
    }

    private void CreateToken()
    {
        if (_checkValueSequence.isAlive)
        {
            _checkValueSequence.Stop();
            _cts.Cancel();
        }

        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
    }
}
