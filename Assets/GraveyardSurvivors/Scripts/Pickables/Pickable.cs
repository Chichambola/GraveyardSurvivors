using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Pickable : MonoBehaviour, IThrowable, IPoolable<Pickable>, IPickable, IFollower
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;

    [SerializeField] private int _value = 1;
    [SerializeField] private float _timeLyingOnGround = 75f;
    [SerializeField] private float _timeChasingTarget = 7f;
    
    public event Action<Pickable> CanBeReleased;
    
    private float _minRandomValue = -1f;
    private float _maxRandomValue = 3f;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private ITarget _target;
    private CancellationTokenSource _cts;
    private UniTask _task;

    public int Value => _value;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _thrower.FinishedMoving += OnFinishedMoving;
        _collider.enabled = false;
    }

    private void OnDisable()
    {
        _thrower.FinishedMoving -= OnFinishedMoving;
        
        _cts?.Cancel();
    }

    private void OnDestroy()
    {
        _cts?.Dispose();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ITarget target) || target != _target)
            return;
        
        Release();
    }

    public void StartThrowing()
    {
        _endPoint.position = _endPoint.position.GetRandomOffsetPosition(_minRandomValue, _maxRandomValue);
        
        _thrower.StartMoving(transform, _endPoint.position);
    }

    public virtual void ResetCharacteristics()
    {
        _collider.enabled = false;

        _target = null;
    }

    public virtual void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    private void OnFinishedMoving()
    {
        _collider.enabled = true;

        _cts = new CancellationTokenSource();
        
        var token = _cts.Token;
        
        WaitTask(_timeLyingOnGround, token, Release).Forget();
    }

    private async UniTaskVoid WaitTask(float time, CancellationToken token ,Action action)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token,  cancelImmediately: true);
        
        action.Invoke();
    }
}
