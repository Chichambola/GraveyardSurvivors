using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageEffect : IEffect<Enemy>
{
    [SerializeField] private float _damageAmount = 1f;
    
    public void Apply(Enemy attacker)
    {
        attacker.TakeDamage(_damageAmount);
    }

    public void Cancel() { }
}
