using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class WeaponWithAbility : Weapon
{
    [SerializeField] private Effect[] _effects;
    [SerializeField] private int _effectChance;
    
    public override event Action<IAttacker, Weapon> AttackerDetected;

    public abstract override void Attack();

    protected void ProcessAttacker(IAttacker attacker)
    {
        if (attacker == null)
            return;
        
        AttackerDetected?.Invoke(attacker, this);

        if (CanEffectProc())
        {
            foreach (var effect in _effects)
            {
                effect.Execute(attacker);
            }
        }
    }

    private bool CanEffectProc()
    {
        float randomNumber = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);

        return _effectChance >= randomNumber;
    }
}
