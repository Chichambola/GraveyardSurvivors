using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

[Serializable]
public class DamageOvertime : IEffect<Enemy>
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _tickInterval = 1f;
    [SerializeField] private float _damagePerTick = 1f;

    private Enemy _currentTarget;
    private IntervalTimer _timer;
    
    public void Apply(Enemy attacker)
    {
        _currentTarget = attacker;
        _timer = new IntervalTimer(_duration, _tickInterval);
        _timer.IntervalReached += OnIntervalReached;
        _timer.TimerStopped += OnTimerStopped;
        _timer.Start();
    }

    public void Cancel()
    {
        _timer?.Stop();
        CleanUp();
    }
    
    private void OnIntervalReached() => _currentTarget?.TakeDamage(_damagePerTick);

    private void OnTimerStopped() => CleanUp();

    private void CleanUp()
    {
        _timer = null;
        _currentTarget = null;
    }
}
