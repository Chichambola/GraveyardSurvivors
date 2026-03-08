using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class Effect
{
    [SerializeReference] private List<IEffectFactory<IAttacker>> _effects = new();

    public virtual void Execute(IAttacker attacker)
    {
        foreach (var effect in _effects)
        {
            var runtimeEffect = effect.Create();
            
            attacker.ApplyEffect(runtimeEffect);
        }
    }
}
