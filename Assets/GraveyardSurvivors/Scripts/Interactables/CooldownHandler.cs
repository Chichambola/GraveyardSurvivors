using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class CooldownHandler : MonoBehaviour
{
    [SerializeField] private float _countdownTime = 1.5f;

    public event Action TimePassed;
    
    private CancellationTokenSource _cts;
    
    public void StartCountdown()
    {
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
        
        CooldownRoutine().Forget();
    }
    
    private async UniTask CooldownRoutine()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_countdownTime), cancellationToken: _cts.Token);
        
        TimePassed?.Invoke();
        
        _cts?.Cancel();
    }
}
