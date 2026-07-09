using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct DamageOvertime : IEffect<IAttacker>
{
    public float Duration;
    public float TickInterval;
    public float DamagePerTick;
    public float EffectChance;
    public ParticleEffectSpawner Effect;

    public event Action<IEffect<IAttacker>> EffectCompleted;
    
    private IAttacker _target;
    private List<ParticleEffect> _currentEffects;
    private IntervalTimer _timer;
    
    public void Apply(IAttacker attacker)
    {
        if (!CanApply())
            return;
        
        _currentEffects = new List<ParticleEffect>();
        
        _target = attacker ?? throw new ArgumentNullException(nameof(attacker));
        
        var targetPosition = _target.CurrentPosition;
        
        var effect = Effect.Spawn(targetPosition,TickInterval);
        
        _currentEffects.Add(effect); 
        
        _timer = new IntervalTimer(Duration, TickInterval);
        _timer.IntervalReached += OnIntervalReached;
        _timer.Stopped += OnTimerStopped;
        _timer.Start();
    }

    public void Cancel()
    {
        _timer?.Stop();
    }
    
    private void OnIntervalReached()
    {
        if (_target != null)
        {
            var targetPosition = _target.CurrentPosition;
            
            var effect = Effect.Spawn(targetPosition);

            _currentEffects.Add(effect);
            
            _target.TakeDamage(DamagePerTick);
        }
    }

    private void OnTimerStopped() => CleanUp();

    private void CleanUp()
    {
        EffectCompleted?.Invoke(this);
        _timer.IntervalReached -= OnIntervalReached;
        _timer.Stopped -= OnTimerStopped;

        foreach (var effect in _currentEffects)
        {
            effect.Release();
        }
        
        _timer = null;
        _target = null;
        _currentEffects = null;
    }
    
    private bool CanApply()
    {
        float randomNumber = Random.Range(UserUtils.s_lowestPercent, UserUtils.s_highestPercent);

        return EffectChance >= randomNumber;
    }
}
