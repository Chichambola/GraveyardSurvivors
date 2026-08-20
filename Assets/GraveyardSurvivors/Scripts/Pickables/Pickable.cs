using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using Tween = PrimeTween.Tween;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Pickable : MonoBehaviour, IThrowable, IPoolable<Pickable>, IPickable, IFollower
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;
    [SerializeField] private Mover _mover;

    [SerializeField] private int _value = 1;
    [SerializeField] private float _timeBeforeRelease = 2f;

    [SerializeField] private TweenSettings<float> _settings;
    
    public event Action<Pickable> CanBeReleased;
    public event Action<Pickable> WasPickedUp;
    
    private float _minRandomValue = -1f;
    private float _maxRandomValue = 3f;
    private IntervalTimer _timer;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private ITarget _target;
    private Sequence _sequence;

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
    }
    
    public void StartMoving()
    {
        _endPoint.position = _endPoint.position.GetRandomOffsetPosition(_minRandomValue, _maxRandomValue);
        
        _thrower.StartMoving(transform, _endPoint.position);
    }

    public void StartMoving(ITarget target)
    {
        _sequence = Sequence.Create().Group(Tween);
    }

    public virtual void ResetCharacteristics()
    {
        _collider.enabled = false;
    }

    public virtual void Release()
    {
        WasPickedUp?.Invoke(this);
    }
    
    private void OnFinishedMoving()
    {
        _collider.enabled = true;
        _timer = new IntervalTimer(_timeBeforeRelease);
        _timer.Stopped += Release;
        _timer.Start();
    }
}
