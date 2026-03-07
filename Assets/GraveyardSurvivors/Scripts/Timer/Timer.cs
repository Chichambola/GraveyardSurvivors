using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class Timer : IDisposable
{
    protected float InitialTime;

    private bool _isDisposed;

    protected Timer(float value)
    {
        InitialTime = value;
    }
    
    public event Action TimerStarted; 
    public event Action TimerStopped; 
    
    public float CurrentTime { get; protected set; }
    public bool IsRunning { get; private set; }
    public abstract bool IsFinished { get; }
    
    public float Progress => Math.Clamp(CurrentTime / InitialTime, 0, 1);
    
    public void Resume() => IsRunning = true;
    
    public void Pause() => IsRunning = false;
    
    public virtual void Reset() => CurrentTime = InitialTime;

    public virtual void Reset(float newTime)
    {
        InitialTime = newTime;
        Reset();
    }

    public void Start()
    {
        CurrentTime = InitialTime;

        if (!IsRunning)
        {
            IsRunning = true;
            TimerController.RegisterTimer(this);
            TimerStarted?.Invoke();
        }
    }

    public void Stop()
    {
        if (IsRunning)
        {
            IsRunning = false;
            TimerController.DeregisterTimer(this);
            TimerStopped?.Invoke();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    public abstract void Tick();

    protected virtual void Dispose(bool isDisposing)
    {
        if(_isDisposed) return;

        if (isDisposing)
        {
            TimerController.DeregisterTimer(this);
        }
        
        _isDisposed = true;
    }
    
    ~Timer()
    {
        Dispose(false);
    }
}