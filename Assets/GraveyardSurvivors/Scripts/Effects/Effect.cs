using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class Effect
{
    [SerializeReference] private List<IEffect<Enemy>> _effects = new();

    public void Execute(Enemy attacker)
    {
        foreach (var effect in _effects)
        {
            effect.Apply(attacker);
        }
    }
}
