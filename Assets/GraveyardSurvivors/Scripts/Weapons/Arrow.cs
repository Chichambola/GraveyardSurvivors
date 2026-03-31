using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    private float _damage;
    
    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    
    public override void Release()
    {
        _damage = 0;
        
        base.Release();
    }
}
