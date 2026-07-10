using System;
using System.Collections;
using System.Collections.Generic;
using Sherbert.Framework.Generic;
using UnityEngine;

public class ParticleEffectHandler : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<EDamageEffectParticle, ParticleEffectSpawner> _damageEffectsPrefabs;
    
    private static SerializableDictionary<EDamageEffectParticle, ParticleEffectSpawner> _damageEffects;

    public static ParticleEffectHandler Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void OnEnable()
    {
        _damageEffects = new SerializableDictionary<EDamageEffectParticle, ParticleEffectSpawner>();
        
        foreach (var effectParticle in _damageEffectsPrefabs.Keys)
        {
            foreach (var spawnerPrefab in _damageEffectsPrefabs.Values)
            {
                var spawner = Instantiate(spawnerPrefab, transform);
                
                _damageEffects.Add(effectParticle, spawner);
            }
        }
    }

    public static ParticleEffectSpawner GetSpawner(EDamageEffectParticle particle)
    {
        return _damageEffects[particle];
    }
}
