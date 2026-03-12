using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _area;
    
    public override event Action<List<IAttacker>> AttackerDetected;

    public override void Execute(float radius = 0f)
    {
        _area.SetSize(radius);
        
        if (_area.TryGetAttackers(out List<IAttacker> detectedAttackers))
        {
            AttackerDetected?.Invoke(detectedAttackers);
        }
    }
}