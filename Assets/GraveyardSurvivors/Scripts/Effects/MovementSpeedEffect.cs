using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementSpeedEffect : IEffect<IAttacker>
{
    public float SpeedPercent;
    public bool IsSlowing;
    
    public event Action<IEffect<IAttacker>> EffectCompleted;
    
    public void Apply(IAttacker attacker)
    {
        attacker.ChangeSpeed(SpeedPercent, IsSlowing);
        EffectCompleted?.Invoke(this);
    }

    public void Cancel() { }
}
