using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectSpawner : Spawner<ParticleEffect>
{
    private float _duration;
    
    public ParticleEffect Spawn(float interval = 1f)
    {
        _duration = interval;
        
        var effect = GetObject();

        return effect;
    }
    
    protected override void ActionOnGet(ParticleEffect effect)
    {
        effect.transform.parent = transform;
        effect.SetDuration(_duration);
        effect.CanBeReleased += Release;
        ActiveObjects.Add(effect);
        
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
