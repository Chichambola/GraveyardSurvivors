using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : ProjectileSpawner
{
    private float _damage;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    
    protected override void ActionOnGet(Projectile projectile)
    {
        if (projectile is Arrow arrow)
        {
            arrow.SetDamage(_damage);
            
            base.ActionOnGet(arrow);
        }
        else
        {
            throw new Exception();
        }
    }
}
