using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : Spawner<Projectile>
{
    [SerializeField] private Transform _gunPoint;
    
    private IAttacker _currentTarget;
    
    public void Spawn(IAttacker target)
    {
        _currentTarget = target;

        GetObject();
    }
    
    protected override void ActionOnGet(Projectile projectile)
    {
        projectile.transform.position = _gunPoint.transform.position;
        
        projectile.CanBeReleased += ActionOnRelease;
        
        base.ActionOnGet(projectile);
        
        projectile.StartMoving(_currentTarget);
    }

    protected override void ActionOnRelease(Projectile projectile)
    {
        projectile.CanBeReleased -= ActionOnRelease;
        
        base.ActionOnRelease(projectile);
    }
}
