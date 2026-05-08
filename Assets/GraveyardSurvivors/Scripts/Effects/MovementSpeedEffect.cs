using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementSpeedEffect : IEffect<IAttacker>, IMovementEffect
{
    public float Duration;
    public float SpeedPercent;
    public bool IsSlowing;
    public ParticleEffectSpawner Effect;
    
    public event Action<IEffect<IAttacker>> EffectCompleted;
    
    private IAttacker _currentTarget;
    private ParticleEffect _currentEffect;
    private IntervalTimer _timer;
    
    public void Apply(IAttacker attacker)
    {
        _currentTarget = attacker ?? throw new ArgumentNullException(nameof(attacker));
        
        _currentEffect = Effect.Spawn(_currentTarget.Rigidbody.position);
        
        _timer = new IntervalTimer(Duration);
        _timer.TimerStopped += OnTimerStopped;
        _timer.TimerStarted += OnTimerStarted;
        _timer.Start();
    }

    public void Cancel()
    {
        _timer?.Stop();
    }

    private void CleanUp()
    {
        EffectCompleted?.Invoke(this);
        
        _timer.TimerStopped -= OnTimerStopped;
        _timer.TimerStarted -= OnTimerStarted;
     
        _currentEffect?.Release();
        
        bool isCurrentlySlowing = !IsSlowing;
        
        _currentTarget.ChangeSpeed(SpeedPercent, isCurrentlySlowing);
        
        _timer = null;
        _currentTarget = null;
        _currentEffect = null;
    }
    
    private void OnTimerStopped() => CleanUp();
    
    private void OnTimerStarted()
    {
        _currentTarget.ChangeSpeed(SpeedPercent, IsSlowing);
    }
}
