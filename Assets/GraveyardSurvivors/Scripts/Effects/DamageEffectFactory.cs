using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageEffectFactory : IEffectFactory<IAttacker>
{
    [SerializeField] private float _damageAmount = 1f;
    
    public IEffect<IAttacker> Create()
    {
        return new DamageEffect { DamageAmount = _damageAmount };
    }
}