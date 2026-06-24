using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _area;
    [SerializeField] private float _radiusPercentGain = 5f;
    
    public override event Action<IAttacker> AttackerDetected;

    public float RadiusPercentGain => _radiusPercentGain;

    public override void Execute()
    {
        _area.SetSize();
        
        if (_area.TryGetAttackers(out List<IAttacker> detectedAttackers))
        {
            foreach (var attacker in detectedAttackers)
            {
                AttackerDetected?.Invoke(attacker);   
            }
        }
    }

    public override void Upgrade()
    {
        _area.AddMultiplier(_radiusPercentGain);
    }
}