using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpawner : Spawner<Projectile>
{
    [SerializeField] private Transform _gunPoint;
    
    public event Action<ITarget> ProjectileHitEnemy;
    
    private ITarget _currentTarget;
    
    public void Spawn(ITarget target)
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
        projectile.HitEnemy += OnEnemyHit;
        
        base.ActionOnGet(projectile);
        
        projectile.SetTarget(_currentTarget);
        
        projectile.StartMoving();
    }

    protected override void ActionOnRelease(Projectile projectile)
    {
        projectile.gameObject.transform.parent = transform;
        
        projectile.CanBeReleased -= Release;
        projectile.HitEnemy -= OnEnemyHit;
        
        ActiveObjects.Remove(projectile);
        
        base.ActionOnRelease(projectile);
        
        projectile.ResetCharacteristics();
    }
    
    private void OnEnemyHit(Projectile projectile)
    {
        ProjectileHitEnemy?.Invoke(projectile.Target);
        
        projectile.Release();
    }
}
