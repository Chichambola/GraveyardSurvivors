using System;
using UnityEngine;

[Serializable]
public class DamageOverTimeFactory : IEffectFactory<IAttacker>
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _tickInterval = 1f;
    [SerializeField] private float _damagePerTick = 1f;
    [SerializeField] private ParticleEffectSpawner _damageEffect;
    
    public IEffect<IAttacker> Create()
    {
        return new DamageOvertime
        {
            Duration = _duration,
            TickInterval = _tickInterval,
            DamagePerTick = _damagePerTick,
            Effect = _damageEffect
        };
    }
}