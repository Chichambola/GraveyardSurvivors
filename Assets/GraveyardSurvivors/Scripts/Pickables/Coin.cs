using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour, IThrowable, IPoolable<Coin>, IPickable
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;

    [SerializeField] private int _value = 1;
    [SerializeField] private float _timeBeforeRelease = 2f;
    
    private Color _originalColor;
    
    public event Action<Coin> CanBeReleased;
    
    private Vector3 _initialForwardRotation;
    private IntervalTimer _timer;
    
    public int Value => _value;
    
    private void OnEnable()
    {
        _initialForwardRotation = transform.forward;

        _thrower.FinishedMoving += OnFinishedMoving;
    }

    private void OnDisable()
    {
        _thrower.FinishedMoving -= OnFinishedMoving;
    }

    public void ResetCharacteristics()
    {
        transform.forward = _initialForwardRotation;
    }

    public void Release()
    {
        _thrower.StopMoving();
        
        CanBeReleased?.Invoke(this);
    }

    public void StartMoving()
    {
        _thrower.StartMoving(transform, _endPoint.position);
    }

    private void OnFinishedMoving()
    {
        _timer = new IntervalTimer(_timeBeforeRelease);
        _timer.TimerStopped += Release;
        _timer.Start();
    }
}
