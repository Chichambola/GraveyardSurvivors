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

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void StartCountdown()
    {
        _cts = new CancellationTokenSource();
        
        var token = _cts.Token;
        
        CooldownRoutine(token).Forget();
    }
    
    private async UniTaskVoid CooldownRoutine(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_countdownTime), cancellationToken: token);
        
        TimePassed?.Invoke();
        
        _cts?.Cancel();
    }
}
