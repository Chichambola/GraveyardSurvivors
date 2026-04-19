using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _area;
    
    public override event Action<IAttacker> AttackerDetected;

    public override void Execute(float radiusMultiplier)
    {
        _area.SetSize();
        _area.AddMultiplier(radiusMultiplier);
        
        if (_area.TryGetAttackers(out List<IAttacker> detectedAttackers))
        {
            foreach (var attacker in detectedAttackers)
            {
                AttackerDetected?.Invoke(attacker);   
            }
        }
    }
}