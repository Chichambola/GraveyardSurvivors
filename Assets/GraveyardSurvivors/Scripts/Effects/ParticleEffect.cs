using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : MonoBehaviour, IPoolable<ParticleEffect>
{
    private ParticleSystem _particleSystem;
    private UniTask _task;
    private float _duration;
    private CancellationTokenSource _cts;
    
    public event Action<ParticleEffect> CanBeReleased;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void ResetCharacteristics()
    {
        _particleSystem.Stop();
        
        _duration = 0;
    }

    private void OnDisable()
    {
        _cts?.Cancel();
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void SetDuration(float duration)
    {
        _particleSystem.Stop();
        
        if(_particleSystem.isPlaying)
            return;
        
        var systemMain = _particleSystem.main;

        systemMain.duration = duration;
        
        var curve = systemMain.startLifetime;
        curve.constant = duration;
        systemMain.startLifetime = curve;
        
        _duration = duration;
    }

    public void SetRadius(float radius)
    {
        float offset = 2f;
        
        _particleSystem.Stop();
        
        if(_particleSystem.isPlaying)
            return;
        
        var systemShape = _particleSystem.shape;

        systemShape.radius = radius / offset;
    }
    
    public void StartPlaying()
    {
        _cts = new CancellationTokenSource();

        var token = _cts.Token;

        WaitTask(_duration, token, Release).Forget();
        
        _particleSystem.Play();
    }
    
    private async UniTaskVoid WaitTask(float time, CancellationToken token, Action action)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token,  cancelImmediately: true).SuppressCancellationThrow();
        
        action.Invoke();
    }
}
