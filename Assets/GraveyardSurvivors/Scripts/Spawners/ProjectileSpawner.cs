using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : Spawner<Projectile>
{
    [SerializeField] private Transform _gunPoint;
    
    private IAttacker _currentTarget;
    private float _damage;
    
    public void Spawn(IAttacker target, float damage)
    {
        _currentTarget = target;
        _damage = damage;

        GetObject();
    }
    
    protected override void ActionOnGet(Projectile projectile)
    {
        ActiveObjects.Add(projectile);
        
        projectile.transform.position = _gunPoint.transform.position;
        projectile.gameObject.transform.parent = null;
        
        projectile.CanBeReleased += Release;
        
        projectile.SetTarget(_currentTarget);
        projectile.SetDamage(_damage);
        
        base.ActionOnGet(projectile);
        
        projectile.StartMoving();
    }

    protected override void ActionOnRelease(Projectile projectile)
    {
        ActiveObjects.Remove(projectile);
        
        projectile.gameObject.transform.parent = transform;
        
        projectile.CanBeReleased -= Release;
        
        base.ActionOnRelease(projectile);
    }
}
