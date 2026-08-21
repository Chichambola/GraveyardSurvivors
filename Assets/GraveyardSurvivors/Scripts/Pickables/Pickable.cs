using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Pickable : MonoBehaviour, IThrowable, IPoolable<Pickable>, IPickable, IFollower
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;
    [SerializeField] private Mover _mover;

    [SerializeField] private int _value = 1;
    [SerializeField] private float _timeBeforeRelease = 2f;

    [SerializeField] private TweenSettings<Vector3> _settings;
    [SerializeField] private ShakeSettings _shakeSettings;
    
    public event Action<Pickable> CanBeReleased;
    public event Action<Pickable> PickedUp;
    
    private float _minRandomValue = -1f;
    private float _maxRandomValue = 3f;
    private float _step = 0.005f;
    private int _amountOfCycles = -1;
    private IntervalTimer _timer;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private ITarget _target;
    private Sequence _sequence;
    private CancellationTokenSource _cts;

    public int Value => _value;
    public bool WasPickedUp { get; private set; }

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
    }

    public void StartMoving()
    {
        _endPoint.position = _endPoint.position.GetRandomOffsetPosition(_minRandomValue, _maxRandomValue);
        
        _thrower.StartMoving(transform, _endPoint.position);
    }

    public async UniTaskVoid StartMoving(ITarget target)
    {
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
        
        _target = target;
        
        _settings.startValue = transform.position;

        _sequence = Sequence.Create(cycles: _amountOfCycles)
            .Group(Tween.Delay(_step, () => _settings.endValue = _target.CurrentPosition)
                .Group(Tween.Position(transform, _settings)));
        
        //_settings.endValue = _target.CurrentPosition
        
        await _sequence.ToYieldInstruction().ToUniTask(PlayerLoopTiming.FixedUpdate, cancellationToken: _cts.Token);
    }

    private void Move()
    {
        _mover.MoveToPosition(_target.CurrentPosition);
    }
    
    public virtual void ResetCharacteristics()
    {
        _collider.enabled = false;
    }

    public virtual void Release()
    {
        if (WasPickedUp)
            return;
        
        WasPickedUp = true;
        
        PickedUp?.Invoke(this);
    }
    
    private void OnFinishedMoving()
    {
        _collider.enabled = true;
        
        _timer = new IntervalTimer(_timeBeforeRelease);
        _timer.Stopped += Release;
        _timer.Start();
    }
}
