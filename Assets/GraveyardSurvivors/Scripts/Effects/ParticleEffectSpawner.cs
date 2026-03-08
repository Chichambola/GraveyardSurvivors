using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectSpawner : Spawner<ParticleEffect>
{
    private GameObject _parentObject;
    
    public void Spawn(float interval, GameObject gameObject)
    {
        _parentObject = gameObject;
        
        GetObject();
    }

    public void Spawn(GameObject gameObject)
    {
        _parentObject = gameObject;
        
        GetObject();
    }
    
    protected override void ActionOnGet(ParticleEffect effect)
    {
        effect.transform.parent = _parentObject.transform;
        effect.CanBeReleased += Release;
        ActiveObjects.Add(effect);
        
        base.ActionOnGet(effect);
    }

    protected override void ActionOnRelease(ParticleEffect effect)
    {
        effect.CanBeReleased -= Release;
        ActiveObjects.Remove(effect);
        
        base.ActionOnRelease(effect);
    }
}
