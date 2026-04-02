using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementSpeedEffectFactory : IEffectFactory<IAttacker>
{
    [SerializeField] private float _speedPercent;
    [SerializeField] private bool _isSlowing;
    
    public IEffect<IAttacker> Create()
    {
        return new MovementSpeedEffect()
        {
            SpeedPercent = _speedPercent,
            IsSlowing = _isSlowing
        };
    }
}
