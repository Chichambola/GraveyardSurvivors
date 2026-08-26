using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectSpawner : Spawner<ParticleEffect>
{
    private Vector3 _spawnPosition;
    private float _duration;
    private float _radius;

    public ParticleEffect Spawn(Vector3 position, float radius = 1f, float duration = 1f)
    {
        _duration = duration;
        _spawnPosition = position;
        _radius = radius;
        
        return GetObject();
    }
    
    protected override void ActionOnGet(ParticleEffect effect)
    {
        effect.transform.position = _spawnPosition;
        effect.CanBeReleased += Release;
        ActiveObjects.Add(effect);
        
        effect.SetDuration(_duration);
        effect.SetRadius(_radius);
        
        base.ActionOnGet(effect);
        
        effect.StartPlaying();
    }

    protected override void ActionOnRelease(ParticleEffect effect)
    {
        effect.CanBeReleased -= Release;
        effect.ResetCharacteristics();
        ActiveObjects.Remove(effect);
        
        base.ActionOnRelease(effect);
    }
}
