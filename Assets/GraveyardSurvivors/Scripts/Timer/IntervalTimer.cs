using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalTimer : Timer
{
    private readonly float _interval;
    private float _nextInterval;

    public event Action IntervalReached;

    public IntervalTimer(float totalTime, float intervalSeconds) : base(totalTime)
    {
        _nextInterval = totalTime - _interval;
        _interval = intervalSeconds;
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

    public override bool IsFinished => CurrentTime <= 0;
}
