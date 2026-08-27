using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

public class Follower : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    
    private Sequence _sequence;
    private CancellationTokenSource _cts;
    private ITarget _target;

    private void OnDisable()
    {
        _cts?.Cancel();
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }
    
    public void StartMoving()
    {
        _cts = new CancellationTokenSource();

        var token = _cts.Token;
        
        MoveTask(token).Forget();
    }

    public void StopMoving()
    {
        _cts?.Cancel();
    }

    private async UniTaskVoid MoveTask(CancellationToken token)
    {
        while (_target.IsAlive && !_cts.IsCancellationRequested)
        {
            _mover.MoveToPosition(_target.CurrentPosition);

            Vector3 distance = _target.CurrentPosition - transform.position;

            Vector3 direction = new Vector3(distance.x, 0f, distance.z).normalized;
            
            _rotator.Rotate(direction);

            await UniTask.WaitForFixedUpdate(token);
        }
        
        StopMoving();
    }
}
