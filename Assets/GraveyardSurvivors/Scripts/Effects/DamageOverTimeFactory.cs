using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class DamageOverTimeFactory : IEffectFactory<IAttacker>
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _tickInterval = 1f;
    [SerializeField] private float _damagePerTick = 1f;
    [SerializeField] private float _effectChance = 10f;
    [SerializeField] private ParticleEffectSpawner _damageEffect;

    public float Chance => _effectChance;
    public ParticleEffectSpawner Spawner => _damageEffect;
    
    public void SetParticleEffectSpawner(ParticleEffectSpawner spawner)
    {
        _damageEffect = spawner;
    }

    public IEffect<IAttacker> Create()
    {
        return new DamageOvertime
        {
            Duration = _duration,
            TickInterval = _tickInterval,
            DamagePerTick = _damagePerTick,
            EffectChance = _effectChance,
            Effect = _damageEffect
        };
    }
}