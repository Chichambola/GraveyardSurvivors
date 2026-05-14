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
    
    private IAttacker _target;
    private ParticleEffect _currentEffect;
    private IntervalTimer _timer;
    
    public void Apply(IAttacker attacker)
    {
        _target = attacker ?? throw new ArgumentNullException(nameof(attacker));

        var targetPosition = _target.CurrentPosition;
        
        _currentEffect = Effect.Spawn(targetPosition);
        
        _timer = new IntervalTimer(Duration);
        _timer.Stopped += OnTimerStopped;
        _timer.Started += OnTimerStarted;
        _timer.Start();
    }

    public void Cancel()
    {
        _timer?.Stop();
    }

    private void CleanUp()
    {
        EffectCompleted?.Invoke(this);
        
        _timer.Stopped -= OnTimerStopped;
        _timer.Started -= OnTimerStarted;
     
        _currentEffect?.Release();
        
        bool isCurrentlySlowing = !IsSlowing;
        
        _target.ChangeSpeed(SpeedPercent, isCurrentlySlowing);
        
        _timer = null;
        _target = null;
        _currentEffect = null;
    }
    
    private void OnTimerStopped() => CleanUp();
    
    private void OnTimerStarted()
    {
        _target.ChangeSpeed(SpeedPercent, IsSlowing);
    }
}
