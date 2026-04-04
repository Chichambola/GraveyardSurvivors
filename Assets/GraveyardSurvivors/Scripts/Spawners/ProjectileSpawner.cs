using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpawner : Spawner<Projectile>
{
    [SerializeField] private Transform _gunPoint;
    
    public event Action<IAttacker> ProjectileReleased;
    
    private IAttacker _currentTarget;
    
    public void Spawn(IAttacker target)
    {
        _currentTarget = target;

        GetObject();
    }
    
    protected override void ActionOnGet(Projectile projectile)
    {
        ActiveObjects.Add(projectile);
        
        projectile.transform.position = _gunPoint.transform.position;
        projectile.gameObject.transform.parent = null;
        
        projectile.CanBeReleased += Release;
        
        projectile.SetTarget(_currentTarget);
        
        base.ActionOnGet(projectile);
        
        projectile.StartMoving();
    }

    protected override void ActionOnRelease(Projectile projectile)
    {
        projectile.gameObject.transform.parent = transform;
        
        projectile.CanBeReleased -= Release;
        
        ActiveObjects.Remove(projectile);
        
        base.ActionOnRelease(projectile);
        
        ProjectileReleased?.Invoke(projectile.CurrentTarget);
        
        projectile.ResetCharacteristics();
    }
}
