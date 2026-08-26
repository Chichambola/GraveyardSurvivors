using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntervalTimer : Timer
{
    private readonly float _interval;
    private float _nextInterval;

    public event Action IntervalReached;
    
    public override bool IsFinished => CurrentTime <= 0;

    public IntervalTimer(float totalTime, float intervalSeconds = 1f) : base(totalTime)
    {
        _interval = intervalSeconds;
        _nextInterval = totalTime - _interval;
    }

    public override void Tick()
    {
        if (IsRunning && CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;

            while (CurrentTime <= _nextInterval && _nextInterval >= 0)
            {
                IntervalReached?.Invoke();
                _nextInterval -= _interval;
            }
        }

        if (IsRunning && CurrentTime <= 0)
        {
            CurrentTime = 0;
            Stop();
        }
    }
}
