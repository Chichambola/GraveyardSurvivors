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
    private List<ParticleEffect> _currentEffects;
    private IntervalTimer _timer;
    
    public void Apply(IAttacker attacker)
    {
        _currentEffects = new List<ParticleEffect>();
        
        _currentTarget = attacker ?? throw new ArgumentNullException(nameof(attacker));
        
        var targetPosition = _currentTarget.GetPosition();
        
        var effect = Effect.Spawn(targetPosition,TickInterval);
        
        _currentEffects.Add(effect); 
        
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
            var targetPosition = _currentTarget.GetPosition();
            
            var effect = Effect.Spawn(targetPosition);

            _currentEffects.Add(effect);
            
            _currentTarget.TakeDamage(DamagePerTick);
        }
    }

    private void OnTimerStopped() => CleanUp();

    private void CleanUp()
    {
        EffectCompleted?.Invoke(this);
        _timer.IntervalReached -= OnIntervalReached;
        _timer.TimerStopped -= OnTimerStopped;

        foreach (var effect in _currentEffects)
        {
            effect.Release();
        }
        
        _timer = null;
        _currentTarget = null;
        _currentEffects = null;
    }
}
