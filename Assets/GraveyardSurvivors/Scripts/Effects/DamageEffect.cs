using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct DamageEffect : IEffect<IAttacker>
{
    public float DamageAmount;
    
    public event Action<IEffect<IAttacker>> EffectCompleted;
    
    public void Apply(IAttacker attacker)
    {
        attacker.TakeDamage(DamageAmount);
        EffectCompleted?.Invoke(this);
    }

    public void Cancel() { }
}
