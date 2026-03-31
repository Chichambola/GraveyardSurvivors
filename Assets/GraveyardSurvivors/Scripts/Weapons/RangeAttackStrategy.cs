using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackStrategy : AttackStrategy
{
    public override event Action<IAttacker> AttackerDetected;

    public override void Execute(float radius = 0)
    {
        
    }
}
