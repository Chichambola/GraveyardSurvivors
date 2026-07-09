using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class WeaponWithAbility : Weapon
{
    [SerializeField] private List<Effect> _effects;
    
    public override event Action<IAttacker, Weapon> AttackerDetected;

    protected void ProcessAttacker(IAttacker attacker)
    {
        if (attacker == null)
            throw new Exception($"{nameof(attacker)} can not be null");
        
        AttackerDetected?.Invoke(attacker, this);
        
        foreach (var effect in _effects)
        {
            effect.Execute(attacker);
        }
    }

    public void AddEffect(Effect effect)
    {
        _effects.Add(effect);
    }
}
