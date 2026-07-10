using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class MovementSpeedEffectFactory : IEffectFactory<IAttacker>
{
    [SerializeField] private float _duration;
    [SerializeField] private float _speedPercent;
    [SerializeField] private bool _isSlowing;
    [SerializeField] private ParticleEffectSpawner _movementEffect;
    
    public void SetParticleEffectSpawner(ParticleEffectSpawner spawner)
    {
        _movementEffect = spawner;
    }

    public IEffect<IAttacker> Create()
    {
        return new MovementSpeedEffect()
        {
            Duration = _duration,
            SpeedPercent = _speedPercent,
            IsSlowing = _isSlowing,
            Effect = _movementEffect
        };
    }
}
