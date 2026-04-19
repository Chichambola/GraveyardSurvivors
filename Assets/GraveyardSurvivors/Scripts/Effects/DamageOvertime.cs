using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct DamageOvertime : IEffect<IAttacker>
{
    public float Duration;
    public float TickInterval;
    public float DamagePerTick;
    public ParticleEffectSpawner Effect;

    public event Action<IEffect<IAttacker>> EffectCompleted;
    
    private IAttacker _currentTarget;
    private ParticleEffect _currentEffect;
    private IntervalTimer _timer;
    
    public void Apply(IAttacker attacker)
    {
        _currentTarget = attacker ?? throw new ArgumentNullException(nameof(attacker));
        
        var target = _currentTarget as MonoBehaviour;
        
        Effect.Spawn(target.transform.position,TickInterval);
        
        _timer = new IntervalTimer(Duration, TickInterval);
        _timer.IntervalReached += OnIntervalReached;
        _timer.TimerStopped += OnTimerStopped;
        _timer.Start();
    }

    public void Cancel()
    {
        _timer?.Stop();
    }
    
    private void OnIntervalReached()
    {
        if (_currentTarget != null)
        {
            var target = _currentTarget as MonoBehaviour;
            
            Effect.Spawn(target.transform.position);

            _currentTarget.TakeDamage(DamagePerTick);
        }
    }

    private void OnTimerStopped() => CleanUp();

    private void CleanUp()
    {
        EffectCompleted?.Invoke(this);
        _timer.IntervalReached -= OnIntervalReached;
        _timer.TimerStopped -= OnTimerStopped;
        
        _currentEffect.Release();
        
        _timer = null;
        _currentTarget = null;
    }
}
