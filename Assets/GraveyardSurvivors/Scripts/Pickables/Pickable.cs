using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour, IThrowable, IPoolable<Pickable>, IPickable
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;

    [SerializeField] private int _value = 1;
    [SerializeField] private float _timeBeforeRelease = 2f;
    
    public abstract event Action<Pickable> CanBeReleased;
    
    private IntervalTimer _timer;
    
    public int Value => _value;
    
    private void OnEnable()
    {
        _thrower.FinishedMoving += OnFinishedMoving;
    }

    private void OnDisable()
    {
        _thrower.FinishedMoving -= OnFinishedMoving;
    }
    
    public void StartMoving()
    {
        _thrower.StartMoving(transform, _endPoint.position, true);
    }

    public virtual void ResetCharacteristics()
    {
        
    }

    public virtual void Release()
    {
        _thrower.StopMoving();
    }
    
    private void OnFinishedMoving()
    {
        _timer = new IntervalTimer(_timeBeforeRelease);
        _timer.Stopped += Release;
        _timer.Start();
    }
}
